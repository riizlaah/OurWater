package nr.dev.ourwater2

import android.content.ContentResolver
import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.graphics.BitmapFactory
import android.net.Uri
import android.provider.OpenableColumns
import android.util.Log
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.core.content.edit
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.json.JSONArray
import org.json.JSONObject
import java.io.ByteArrayOutputStream
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
    val bytes: ByteArray? = null,
    val timeout: Int = 10000
)

data class HttpRes(
    val code: Int,
    val body: String? = null,
    val bytes: ByteArray? = null,
    val errors: String? = null
)

data class File(
    val name: String,
    val bytes: ByteArray,
    val mimetype: String = "application/octet-stream"
)


data class User(
    val id: Int,
    val username: String,
    val fullname: String,
    val role: String,
    val address: String
)

data class ConsumptionDebit(
    val id: Int,
    val customerName: String,
    val inputtedBy: String,
    val inputtedByRole: String,
    val correctedBy: String?,
    val debit: Double,
    val date: LocalDate,
    val status: String,
    val location: String,
    val updatedAt: LocalDateTime,
    val prevDebit: Double? = null,
    val imagePath: String = "",
    val rejectionReason: String = ""
)

data class ShortConsumptionDebit(
    val id: Int,
    val debit: Double
)

data class Customer(
    val name: String,
    val address: String = ""
)

data class Bill(
    val id: Int,
    val consumptionDebit: ShortConsumptionDebit,
    val customer: Customer,
    val originalAmount: Double,
    val extraFine: Double,
    val totalAmount: Double,
    val deadline: LocalDateTime,
    val status: String,
    val createdAt: LocalDateTime,
    val fines: List<String> = emptyList(),
    val rejectionReason: String = "",
    val imagePath: String? = null,
)

data class Customer2(
    val id: Int,
    val name: String,
    val phoneNumber: String,
    val address: String
)

data class SubmittedConsumptionDebit(
    val id: Int,
    val debit: Double,
    val status: String,
    val imagePath: String?,
    val rejectionReason: String,
    val customer: Customer2
)

object HttpClient {
    const val addr = "http://10.0.2.2:5000"

    var user by mutableStateOf<User?>(null)

    var token = ""

    lateinit var sharedPrefs: SharedPreferences

    fun loadToken() {
        token = sharedPrefs.getString("token", "") ?: ""
    }

    fun saveToken() {
        sharedPrefs.edit(commit = true) {
            putString("token", token)
        }
    }

    fun send(req: HttpReq, getByte: Boolean = false): HttpRes {
        val conn = URL(req.url).openConnection() as HttpURLConnection
        return try {
            conn.requestMethod = req.method
            conn.readTimeout = req.timeout
            conn.connectTimeout = req.timeout
            req.headers.forEach { (k, v) -> conn.setRequestProperty(k, v) }
            if ((req.body.isNotEmpty() || req.bytes != null) && req.method in listOf(
                    "POST",
                    "PUT",
                    "PATCH"
                )
            ) {
                conn.getOutputStream().buffered()
                    .use { it.write(req.bytes ?: req.body.toByteArray()) }
            }

            conn.connect()
            val code = conn.responseCode
            val body = if (getByte) null else {
                if (code in 200..299) {
                    conn.getInputStream().bufferedReader().use { it.readText() }
                } else {
                    conn.errorStream?.bufferedReader()?.use { it.readText() }
                }
            }
            val bytes = if (!getByte) null else {
                if (code in 200..299) {
                    conn.getInputStream().buffered().use { it.readBytes() }
                } else {
                    conn.errorStream?.buffered()?.use { it.readBytes() }
                }
            }
            HttpRes(code, body, bytes)
        } catch (e: Exception) {
            HttpRes(code = -1, errors = e.message ?: "Network error")
        }
    }

    suspend fun jsonReq(route: String, body: String = "", method: String = "GET"): HttpRes {
        val headers = if (token.isNotEmpty()) mapOf(
            "content-type" to "application/json",
            "authorization" to "Bearer $token"
        ) else mapOf("content-type" to "application/json")
        val res = withContext(Dispatchers.IO) {
            send(HttpReq("$addr/api/$route", body, method, headers))
        }
        return res
    }

    suspend fun fetchImg(route: String): ImageBitmap? {
        return try {
            val res = withContext(Dispatchers.IO) {
                send(HttpReq("$addr/uploads/$route"), true)
            }
            if (res.code != 200 || res.bytes == null) null
            else {
                val bitmap = withContext(Dispatchers.IO) {
                    BitmapFactory.decodeByteArray(res.bytes, 0, res.bytes.size)
                }
                bitmap.asImageBitmap()
            }
        } catch (e: Exception) {
            Log.e("ImgFetcher", e.message ?: "Unknown error")
            null
        }
    }

    suspend fun sendMultipart(
        route: String,
        files: Map<String, File>,
        others: Map<String, String> = emptyMap(),
        method: String = "POST"
    ): HttpRes {
        return try {
            val boundary = "----formBoundary${System.currentTimeMillis()}"
            val boundaryBytes = boundary.toByteArray()
            val crlf = "\r\n".toByteArray()
            val twoH = "--".toByteArray()
            val outputStream = ByteArrayOutputStream()
            others.forEach { (k, v) ->
                outputStream.run {
                    write(twoH)
                    write(boundaryBytes)
                    write(crlf)
                    write("Content-Disposition: form-data; name=\"$k\"".toByteArray())
                    write(crlf)
                    write(crlf)
                    write(v.toByteArray())
                    write(crlf)
                }
            }
            files.forEach { (k, file) ->
                outputStream.run {
                    write(twoH)
                    write(boundaryBytes)
                    write(crlf)
                    write("Content-Disposition: form-data; name=\"$k\"; filename=\"${file.name}\"".toByteArray())
                    write(crlf)
                    write("Content-Type: ${file.mimetype}".toByteArray())
                    write(crlf)
                    write(crlf)
                    write(file.bytes)
                    write(crlf)
                }
                outputStream.run {
                    write(twoH)
                    write(boundaryBytes)
                    write(twoH)
                    write(crlf)
                }
            }

            val bytes = outputStream.toByteArray()
            var headers = mapOf("Content-Type" to "multipart/form-data; boundary=$boundary")
            if (token.isNotEmpty()) headers = headers + mapOf("authorization" to "Bearer $token")
            withContext(Dispatchers.IO) {
                send(HttpReq("$addr/api/$route", bytes = bytes, headers = headers, method = method))
            }
        } catch (e: Exception) {
            HttpRes(-1, errors = e.message ?: "Upload failed")
        }
    }

    suspend fun login(username: String, password: String): String {
        val res =
            jsonReq("users/login", """{"username": "$username", "password": "$password"}""", "POST")
        if (res.body == null) return "Login failed"
        return try {
            val json = JSONObject(res.body)
            if (res.code == 200) {
                if (json.getJSONObject("data").getString("role") == "admin") {
                    "Not for admin"
                } else {
                    token = json.getJSONObject("data").getString("token")
                    saveToken()
                    me()
                    "ok"
                }
            } else {
                json.optString("message", "Login Failed")
            }
        } catch (e: Exception) {
            Log.d("Login", e.message ?: "Unknown error")
            "Login failed"
        }
    }

    suspend fun me(): Boolean {
        val res = jsonReq("users/me")
        if (res.body == null) return true
        return try {
            val json = JSONObject(res.body).getJSONObject("data")
            user = User(
                json.getInt("id"),
                json.getString("username"),
                json.getString("fullname"),
                json.getString("role"),
                json.getString("address"),
            )
            res.code == 200
        } catch (e: Exception) {
            e.printStackTrace()
            false
        }
    }

    suspend fun getConsumptionDebits(): List<ConsumptionDebit> {
        val res = jsonReq("consumptiondebits")
        if (res.body == null || res.code != 200) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        return json.bindList<ConsumptionDebit>()
    }

    suspend fun getConsumptionDebit(id: Int): ConsumptionDebit? {
        val res = jsonReq("consumptiondebits/$id")
        if (res.body == null || res.code != 200) return null
        return try {
            val json = JSONObject(res.body).getJSONObject("data")
            json.bind<ConsumptionDebit>()
        } catch (e: Exception) {
            e.printStackTrace()
            null
        }
    }

    suspend fun getBills(): List<Bill> {
        val res = jsonReq("bills")
        if (res.body == null || res.code != 200) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        return json.bindList<Bill>()
    }

    suspend fun getBill(id: Int): Bill? {
        val res = jsonReq("bills/$id")
        if (res.body == null || res.code != 200) return null
        return try {
            val json = JSONObject(res.body).getJSONObject("data")
            json.bind<Bill>()
        } catch (e: Exception) {
            e.printStackTrace()
            null
        }
    }

    suspend fun getCustomers(search: String): List<Customer2> {
        if (search.trim().isEmpty()) return emptyList()
        val res = jsonReq("users/customers?search=" + withContext(Dispatchers.IO) {
            URLEncoder.encode(search, "UTF-8")
        })
        if (res.body == null || res.code != 200) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        return json.bindList<Customer2>()
    }

    suspend fun getSubmittedConsumptionDebit(id: Int): SubmittedConsumptionDebit? {
        val res = jsonReq("consumptiondebits/customer/$id")
        if (res.body == null || res.code != 200) return null
        return try {
            val json = JSONObject(res.body).getJSONObject("data")
            json.bind<SubmittedConsumptionDebit>()
        } catch (e: Exception) {
            e.printStackTrace()
            null
        }
    }

    suspend fun submitConsumptionDebit(customerId: Int, debit: String, img: File): String {
        val res = sendMultipart(
            "consumptiondebits",
            mapOf("img" to img),
            mapOf("customerId" to customerId.toString(), "debit" to debit.replace('.', ','))
        )
        println(res)
        if(res.code == 200) return "ok"
        if(res.body == null) return "Submit Consumption Debit Failed"
        return try {
            JSONObject(res.body).getString("message")
        } catch (e: Exception) {
            e.printStackTrace()
            "Submit Consumption Debit Failed"
        }
    }

    suspend fun patchConsumptionDebit(id: Int, status: String, rejectionReason: String): String {
        val req = """{"rejectionReason": "$rejectionReason", "status": "$status"}"""
        val res = jsonReq("consumptiondebits/$id", req, "PATCH")
        if(res.code == 200) return "ok"
        if(res.body == null) return "Failed to update consumption debit"
        return try {
            JSONObject(res.body).getString("message")
        } catch (e: Exception) {
            e.printStackTrace()
            "Failed to update consumption debit"
        }
    }

    suspend fun payBill(id: Int, img: File): String {
        val res = sendMultipart(
            "bills/$id/pay",
            mapOf("img" to img)
        )
        if(res.code == 200) return "ok"
        if(res.body == null) return "Bill Payment Failed"
        return try {
            JSONObject(res.body).getString("message")
        } catch (e: Exception) {
            e.printStackTrace()
            "Bill Payment Failed"
        }
    }
}

inline fun <reified T> JSONObject.bind(): T {
    return when (T::class) {
        ConsumptionDebit::class -> {
            val detailed = has("imagePath")
            ConsumptionDebit(
                getInt("id"),
                getString("customerName"),
                getString("inputtedBy"),
                optString("inputtedByRole"),
                if(isNull("correctedBy")) null else getString("correctedBy"),
                getDouble("debit"),
                LocalDate.parse(getString("date")),
                getString("status"),
                getString("location"),
                LocalDateTime.parse(getString("updatedAt")),
                if (detailed && !isNull("prevDebit")) getDouble("prevDebit") else null,
                if (detailed) getString("imagePath") else "",
                if (detailed) getString("rejectionReason") else ""
            )
        }

        Bill::class -> {
            val detailed = has("imagePath")
            val consumptionDebit = getJSONObject("consumptionDebitRecord")
            val customer = getJSONObject("customer")
            Bill(
                getInt("id"),
                ShortConsumptionDebit(
                    consumptionDebit.getInt("id"),
                    consumptionDebit.getDouble("debit")
                ),
                Customer(
                    customer.getString("name"),
                    if (detailed) customer.getString("address") else ""
                ),
                getDouble("originalAmount"),
                getDouble("extraFine"),
                getDouble("totalAmount"),
                LocalDateTime.parse(getString("deadline")),
                getString("status"),
                LocalDateTime.parse(getString("createdAt")),
                if (detailed) getJSONArray("fines").getStringList() else emptyList(),
                if (detailed) getString("rejectionReason") else "",
                if (detailed && !isNull("imagePath")) getString("imagePath") else null
            )
        }

        Customer2::class -> {
            Customer2(
                getInt("id"),
                getString("name"),
                getString("phoneNumber"),
                getString("address")
            )
        }

        SubmittedConsumptionDebit::class -> {
            val cust = getJSONObject("customer")
            SubmittedConsumptionDebit(
                getInt("id"),
                getDouble("debit"),
                getString("status"),
                if (isNull("imagePath")) null else getString("imagePath"),
                getString("rejectionReason"),
                Customer2(
                    cust.getInt("id"),
                    cust.getString("name"),
                    cust.getString("phoneNumber"),
                    cust.getString("address")
                )
            )
        }

        else -> {}
    } as T
}


inline fun <reified T> JSONArray.bindList(): List<T> {
    val arr = mutableListOf<T>()
    for (i in 0 until length()) {
        when (T::class) {
            String::class -> arr.add(getString(i) as T)
            Int::class -> arr.add(getInt(i) as T)
            Double::class -> arr.add(getDouble(i) as T)
            Boolean::class -> arr.add(getBoolean(i) as T)
            else -> {
                val obj = getJSONObject(i)
                arr.add(obj.bind<T>())
            }
        }

    }
    return arr
}

fun JSONArray.getStringList(): List<String> {
    val arr = mutableListOf<String>()
    for (i in 0 until length()) {
        arr.add(getString(i))
    }
    return arr
}

class PickJpgPngContract : ActivityResultContracts.GetContent() {
    override fun createIntent(context: Context, input: String): Intent {
        return super.createIntent(context, input).apply {
            putExtra(Intent.EXTRA_MIME_TYPES, arrayOf("image/jpeg", "image/png"))
        }
    }
}

suspend fun ContentResolver.AsImage(uri: Uri): ImageBitmap? {
    return try {
        withContext(Dispatchers.IO) {
            openInputStream(uri)?.use { stream ->
                val bytes =
                    stream.buffered().use { it.readBytes() }
                BitmapFactory.decodeByteArray(
                    bytes,
                    0,
                    bytes.size
                ).asImageBitmap()
            }
        }
    } catch (e: Exception) {
        e.printStackTrace()
        null
    }
}

suspend fun ContentResolver.getBytes(uri: Uri): ByteArray? {
    return try {
        withContext(Dispatchers.IO) {
            openInputStream(uri)?.use { stream ->
                stream.buffered().use { it.readBytes() }
            }
        }
    } catch (e: Exception) {
        e.printStackTrace()
        null
    }
}

fun ContentResolver.getMimeType(uri: Uri): String {
    return getType(uri) ?: "application/octet-stream"
}

fun ContentResolver.getFilename(uri: Uri): String {
    return when (uri.scheme) {
        "content" -> {
            query(uri, null, null, null, null)?.use { cursor ->
                val nameIndex = cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                if (nameIndex != -1 && cursor.moveToFirst()) {
                    cursor.getString(nameIndex)
                } else null
            } ?: "default"
        }
        "file" -> uri.path?.let { java.io.File(it).name } ?: "default"
        else -> "default"
    }
}
