package nr.dev.ourwater2

import android.content.SharedPreferences
import android.graphics.BitmapFactory
import android.util.Log
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.getValue
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
        others: Map<String, String>
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
            }
            outputStream.write(twoH)
            outputStream.write(boundaryBytes)
            outputStream.write(twoH)
            outputStream.write(crlf)

            val bytes = outputStream.toByteArray()
            var headers = mapOf("Content-Type" to "multipart/form-data; boundary=$boundary")
            if (token.isNotEmpty()) headers = headers + mapOf("authorization" to "Bearer $token")
            withContext(Dispatchers.IO) {
                send(HttpReq("$addr/api/$route", bytes = bytes, headers = headers))
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
                if(json.getJSONObject("data").getString("role") == "admin") {
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
            false
        }
    }

    suspend fun getConsumptionDebits(): List<ConsumptionDebit> {
        val res = jsonReq("consumptiondebits")
        if(res.body == null || res.code != 200) return emptyList()
        val json = JSONObject(res.body).getJSONArray("data")
        return json.bindList<ConsumptionDebit>()
    }

    suspend fun getConsumptionDebit(id: Int): ConsumptionDebit? {
        val res = jsonReq("consumptiondebits/$id")
        if(res.body == null || res.code != 200) return null
        return try {
            val json = JSONObject(res.body).getJSONObject("data")
            json.bind<ConsumptionDebit>()
        } catch (e: Exception) {
            null
        }
    }
}

inline fun <reified T> JSONObject.bind(): T {
    return when(T::class) {
        ConsumptionDebit::class -> {
            val detailed = has("imagePath")
            ConsumptionDebit(
                getInt("id"),
                getString("customerName"),
                getString("inputtedBy"),
                getString("correctedBy"),
                getDouble("debit"),
                LocalDate.parse(getString("date")),
                getString("status"),
                getString("location"),
                LocalDateTime.parse(getString("updatedAt")),
                if(detailed && !isNull("prevDebit")) getDouble("prevDebit") else null,
                if(detailed) getString("imagePath") else "",
                if(detailed) getString("rejectionReason") else ""
            )
        }
        else -> {}
    } as T
}


inline fun <reified T> JSONArray.bindList(): List<T> {
    val arr = mutableListOf<T>()
    for(i in 0 until length()) {
        val obj = getJSONObject(i)
        arr.add(obj.bind<T>())
    }
    return arr
}

