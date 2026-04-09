package nr.dev.ourwater

import android.content.ContentResolver
import android.content.Context
import android.content.SharedPreferences
import android.graphics.BitmapFactory
import android.net.Uri
import android.provider.OpenableColumns
import android.util.Log
import android.webkit.MimeTypeMap
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.compose.runtime.getValue
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.core.content.edit
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import kotlinx.serialization.builtins.ByteArraySerializer
import org.json.JSONObject
import java.io.ByteArrayInputStream
import java.io.ByteArrayOutputStream
import java.io.File
import java.net.HttpURLConnection
import java.net.URL
import java.net.URLEncoder
import java.time.LocalDate
import java.time.LocalDateTime

data class HttpReq(
    val url: String,
    val body: String = "",
    val method: String = "GET",
    val headers: Map<String, String> = emptyMap(),
    val timeout: Int = 10000
)

data class HttpRes(
    val code: Int,
    val body: String? = null,
    val bytes: ByteArray? = null,
    val headers: Map<String, List<String>> = emptyMap(),
    val errors: String? = null
)

data class ConsumptionDebit(
    val id: Int,
    val debit: Double,
    val date: LocalDate,
    val customerName: String = "",
    val inputtedBy: String = "",
    val correctedBy: String? = null,
    val status: String = "",
    val location: String = "",
    val updatedAt: LocalDateTime = LocalDateTime.now(),
    val rejectionReason: String = "",
    val imagePath: String = ""
)

data class Customer(
    val id: Int,
    val name: String
)

data class Bill(
    val id: Int,
    val customerId: Int,
    val consumptionRecord: ConsumptionDebit,
    val status: String,
    val amount: Double,
    val deadline: LocalDateTime,
    val updatedAt: LocalDateTime,
    val createdAt: LocalDateTime,
    val totalAmount: Double = 0.0,
    val fineAmount: Double = 0.0,
    val fines: List<String> = emptyList(),
    val rejectionReason: String = "",
    val imagePath: String = "",
)


data class User(
    val id: Int,
    val fullName: String,
    val role: String
)

data class FileUpload(
    val name: String,
    val content: ByteArray,
    val mimeType: String = "application/octet-stream"
)

object HttpClient {
    val addr = "http://10.0.2.2:5000/"

    var token = ""
    var role = ""

    var user by mutableStateOf<User?>(null)

    lateinit var sharedPrefs: SharedPreferences

    fun loadToken() {

        token = sharedPrefs.getString("token", "") ?: ""
    }

    fun saveToken() {
        sharedPrefs.edit {
            putString("token", token)
        }
    }

    fun send(req: HttpReq, getByte: Boolean = false): HttpRes {
        val conn = URL(req.url).openConnection() as HttpURLConnection
        return try {
            conn.requestMethod = req.method
            conn.readTimeout = req.timeout
            conn.connectTimeout = req.timeout
            req.headers.forEach { (t, u) -> conn.setRequestProperty(t, u) }
            if (req.body.isNotEmpty() && req.method in listOf("POST", "PUT", "PATCH")) {
                conn.getOutputStream().buffered().use { it.write(req.body.toByteArray()) }
            }

            conn.connect()
            val code = conn.responseCode
            val bytes = if (!getByte) {
                null
            } else {
                if (code in 200..299) {
                    conn.getInputStream().buffered().use { it.readBytes() }
                } else {
                    conn.errorStream?.buffered()?.use { it.readBytes() }
                }
            }
            val body = if (getByte) {
                null
            } else {
                if (code in 200..299) {
                    conn.getInputStream().bufferedReader().use { it.readText() }
                } else {
                    conn.errorStream?.bufferedReader()?.use { it.readText() }
                }
            }
            HttpRes(
                code,
                body,
                bytes,
                conn.headerFields
            )
        } catch (e: Exception) {
            HttpRes(-1, errors = e.message ?: "Network Error")
        } finally {
            conn.disconnect()
        }
    }

    suspend fun fetchImg(path: String): ImageBitmap? {
        val res = withContext(Dispatchers.IO) {
            send(HttpReq(addr + "uploads/$path"), true)
        }
        if (res.code != 200 || res.bytes == null) return null
        return try {
            val factory = BitmapFactory.decodeByteArray(res.bytes, 0, res.bytes.size)
            factory.asImageBitmap()
        } catch (e: Exception) {
            Log.e("Image Fetch", e.message ?: "Unknown Error")
            null
        }
    }

    suspend fun jsonReq(route: String, body: String = "", method: String = "GET"): HttpRes {
        val headers = if (token.isEmpty()) mapOf("content-type" to "application/json") else mapOf(
            "content-type" to "application/json",
            "authorization" to "Bearer $token"
        )
        val res = withContext(Dispatchers.IO) {
            send(HttpReq(addr + route, body, method, headers))
        }
        return res
    }

    suspend fun sendWithFile(
        route: String,
        files: Map<String, FileUpload>,
        others: Map<String, String> = emptyMap(),
        method: String = "POST"
    ): HttpRes {
        return try {
            val boundary = "----WebKitFormBoundary${System.currentTimeMillis()}"
            val lineEnd = "\r\n".toByteArray()
            val twoHyphens = "--".toByteArray()

            val outputStream = ByteArrayOutputStream()

            others.forEach { (k, v) ->
                outputStream.write(twoHyphens)
                outputStream.write(boundary.toByteArray())
                outputStream.write(lineEnd)
                outputStream.write("Content-Disposition: form-data; name=\"$k\"".toByteArray())
                outputStream.write(lineEnd)
                outputStream.write(lineEnd)
                outputStream.write(v.toByteArray())
                outputStream.write(lineEnd)
            }
            files.forEach { (name, file) ->
                outputStream.write(twoHyphens)
                outputStream.write(boundary.toByteArray())
                outputStream.write(lineEnd)
                outputStream.write("Content-Disposition: form-data; name=\"$name\"; filename=\"${file.name}\"".toByteArray())
                outputStream.write(lineEnd)
                outputStream.write("Content-Type: ${file.mimeType}".toByteArray())
                outputStream.write(lineEnd)
                outputStream.write(lineEnd)
                outputStream.write(file.content)
                outputStream.write(lineEnd)
            }
            outputStream.write(twoHyphens)
            outputStream.write(boundary.toByteArray())
            outputStream.write(twoHyphens)
            outputStream.write(lineEnd)

            val body = outputStream.toByteArray()

            var headers = mapOf(
                "Content-Type" to "multipart/form-data; boundary=$boundary",
                "Content-Length" to body.size.toString()
            )
            if(token != "") headers = headers + mapOf("authorization" to "Bearer $token")

            withContext(Dispatchers.IO) {
                send(HttpReq(addr + route, String(body), method, headers))
            }
        } catch (e: Exception) {
            HttpRes(-1, errors = e.message ?: "Upload failed")
        }
    }

    suspend fun login(username: String, password: String): String {
        val res =
            jsonReq("api/auth", """{"username": "$username", "password": "$password" }""", "POST")
        if (res.body.isNullOrEmpty()) return "Login Failed"
        return try {
            val json = JSONObject(res.body)
            if (res.code == 200) {
                token = json.getJSONObject("data").getString("token")
                role = json.getJSONObject("data").getString("role")
                saveToken()
                "ok"
            } else {
                json.optString("message", "Login Failed")
            }
        } catch (e: Exception) {
            Log.e("Login", e.message ?: "Error")
            "Login Failed"
        }
    }

    suspend fun me(): Boolean {
        val res = jsonReq("api/me")
        if(res.code != 200 || res.body.isNullOrEmpty()) return false
        val obj = JSONObject(res.body).getJSONObject("data")
        user = User(obj.getInt("id"), obj.getString("fullname"), obj.getString("role"))
        return true
    }

    suspend fun getConsumptionDebitRecords(): List<ConsumptionDebit> {
        val res = jsonReq("api/debit-record")
        if (res.code != 200 || res.body.isNullOrEmpty()) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        val arr = mutableListOf<ConsumptionDebit>()
        for (i in 0 until json.length()) {
            val obj = json.getJSONObject(i)
            arr.add(
                ConsumptionDebit(
                    obj.getInt("id"),
                    obj.getDouble("debit"),
                    LocalDate.parse(obj.getString("date")),
                    obj.getString("customerName"),
                    obj.getString("inputtedBy"),
                    if(obj.isNull("correctedBy")) null else obj.getString("correctedBy"),
                    obj.getString("status"),
                    obj.getString("location"),
                    LocalDateTime.parse(obj.getString("updatedAt")),
                )
            )
        }
        return arr
    }

    suspend fun getConsumptionDebitById(id: Int): ConsumptionDebit? {
        val res = jsonReq("api/debit-record/$id")
        if (res.code != 200 || res.body.isNullOrEmpty()) return null
        val obj = JSONObject(res.body).getJSONObject("data")
        return ConsumptionDebit(
            obj.getInt("id"),
            obj.getDouble("debit"),
            LocalDate.parse(obj.getString("date")),
            obj.getString("customerName"),
            obj.getString("inputtedBy"),
            if(obj.isNull("correctedBy")) null else obj.getString("correctedBy"),
            obj.getString("status"),
            obj.getString("location"),
            LocalDateTime.parse(obj.getString("updatedAt")),
            obj.getString("rejectionReason"),
            obj.getString("imagePath"),
        )
    }

    suspend fun getBills(): List<Bill> {
        val res = jsonReq("api/bills")
        if (res.code != 200 || res.body.isNullOrEmpty()) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        val arr = mutableListOf<Bill>()
        for (i in 0 until json.length()) {
            val obj = json.getJSONObject(i)
            val cdbObj = obj.getJSONObject("consumptionRecord")
            val cdb = ConsumptionDebit(
                cdbObj.getInt("id"), cdbObj.getDouble("debit"),
                LocalDate.parse(cdbObj.getString("date"))
            )
            arr.add(
                Bill(
                    obj.getInt("id"),
                    obj.getInt("customerId"),
                    cdb,
                    obj.getString("status"),
                    obj.getDouble("amount"),
                    LocalDateTime.parse(obj.getString("deadline")),
                    LocalDateTime.parse(obj.getString("updatedAt")),
                    LocalDateTime.parse(obj.getString("createdAt")),
                )
            )
        }
        return arr
    }

    suspend fun getBillById(id: Int): Bill? {
        val res = jsonReq("api/bills/$id")
        if (res.code != 200 || res.body.isNullOrEmpty()) return null
        val obj = JSONObject(res.body).getJSONObject("data")
        val cdbObj = obj.getJSONObject("consumptionRecord")
        val cdb = ConsumptionDebit(
            cdbObj.getInt("id"), cdbObj.getDouble("debit"),
            LocalDate.parse(cdbObj.getString("date"))
        )
        val fines = mutableListOf<String>()
        val objFines = obj.getJSONArray("fines")
        for(i in 0 until objFines.length()) fines.add(objFines.getString(i))
        return Bill(
            obj.getInt("id"),
            obj.getInt("customerId"),
            cdb,
            obj.getString("status"),
            obj.getDouble("originalAmount"),
            LocalDateTime.parse(obj.getString("deadline")),
            LocalDateTime.parse(obj.getString("updatedAt")),
            LocalDateTime.parse(obj.getString("createdAt")),
            obj.getDouble("totalAmount"),
            obj.getDouble("fineAmount"),
            fines,
            obj.getString("rejectionReason"),
            obj.getString("imagePath")
        )
    }

    suspend fun searchCustomers(search: String): List<Customer> {
        val res = jsonReq("api/customers?s=${URLEncoder.encode(search, "UTF-8")}")
        if (res.code != 200 || res.body.isNullOrEmpty()) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        val arr = mutableListOf<Customer>()
        for(i in 0 until json.length()) {
            val obj = json.getJSONObject(i)
            arr.add(Customer(
                obj.getInt("id"),
                obj.getString("name"),
            ))
        }
        return arr
    }

    suspend fun submitPayment() {}

    suspend fun submitConsumptionDebit(file: FileUpload, customerId: Int, debit: Double): String {
        val res = sendWithFile("api/debit-record", mapOf("img" to file), mapOf("customerId" to customerId.toString(), "debit" to debit.toString().replace('.', ',')), "POST")
        println(res)
        if (res.body.isNullOrEmpty()) return "Submit Consumption Debit Record Failed"
        return try {
            val json = JSONObject(res.body)
            if (res.code == 200) {
                "ok"
            } else {
                json.optString("message", "Submit Consumption Debit Record Failed")
            }
        } catch (e: Exception) {
            Log.e("SubmitConsDebitRec", e.message ?: "Error")
            "Submit Consumption Debit Record Failed"
        }
    }

    suspend fun patchConsumptionDebit(id: Int, rejectionReason: String, status: String): Boolean {
        val res = jsonReq("api/debit-record/$id", """{"rejectionReason": "$rejectionReason", "status": "$status" }""", "PATCH")
        return res.code == 200
    }
}

fun Context.getFileName(uri: Uri): String? {
    return when (uri.scheme) {
        "content" -> {
            contentResolver.query(uri, null, null, null, null)?.use { cursor ->
                val nameIndex = cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                if (cursor.moveToFirst()) cursor.getString(nameIndex) else null
            }
        }
        "file" -> uri.path?.let { File(it).name }
        else -> null
    }
}

fun Context.getFileMimeType(uri: Uri): String? {
    return if (ContentResolver.SCHEME_CONTENT == uri.scheme) {
        contentResolver.getType(uri)
    } else {
        val fileExtension = MimeTypeMap.getFileExtensionFromUrl(uri.toString())
        MimeTypeMap.getSingleton().getMimeTypeFromExtension(fileExtension.lowercase())
    }
}