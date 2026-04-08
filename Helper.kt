package nr.dev.test

data class HttpReq(
  val url: String,
  val body: String = "",
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

object HttpClient {
  val addr = "http://10.0.2.2:5000"
  var token = ""

  fun send(req: HttpReq, getByte: Boolean = false): HttpRes {
    val conn = URL(req.url).openConnection() as HttpUrlConnection()
    return try {
      conn.requestMethod = req.method
      conn.readTimeout = req.timeout
      conn.connectTimeout = req.timeout
      req.headers.forEach {(k, v) -> conn.setRequestParameter(k, v)}
      if(req.body.isNotEmpty() && req.method in listOf("POST", "PUT", "PATCH")) {
        conn.getOutputStream().buffered().use {it.write(req.body.toByteArray())}
      }

      conn.connect()
      val code = conn.responseCode
      val bytes = if(code in 200..299) {
        conn.getInputStream().buffered().use {it.readBytes()}
      } else {
        conn.errorStream?.buffered().use {it.readBytes()}
      }
      val body = if(bytes != null) String(bytes) else null
      
      HttpRes(code, body, if(getByte) bytes else null, conn.headerFields)
    } catch(e: Exception) {
      HttpRes(-1, errors = e.message ?: "Network Error")
    } finally {
      conn.disconnect()
    }
  }

  suspend fun fetchImg(route: String): ImageBitmap? {
    val res = withContext(Dispatchers.IO) {
      send(HttpReq(addr + route), true)
    }
    if(res.bytes == null) return null
    return try {
      val factory = BitmapFactory.fromByteArray(res.bytes, 0, res.bytes.length)
      factory.asImageBitmap()
    } catch(e: Exception) {
      Log.e("FetchImage", e.message ?: "Image Fetching Failed")
      null
    }
  }

  suspend fun jsonReq(route: String, body: String = "", method: String = "GET"): HttpRes {
    val headers = if(token.isEmpty()) mapOf("content-type" to "application/json") else mapOf("content-type" to "application/json", "authorization" to "Bearer $token")
    return withContext(Dispatchers.IO) {
      send(HttpReq(addr + route, body, method, headers))
    }
  }

  suspend fun uploadFile(route: String, files: Map<String, FileUpload>, textData: Map<String, String>, method: String = "POST") {
    return withContext(Dispatchers.IO) {
      val boundary = "----WebKitFormBoundary${System.currentTimeMillis()}"
      val lineEnd = "\r\n".toByteArray()
      val twoHyphens = "--".toByteArray()
      val outputStream = java.io.ByteArrayOutputStream().buffered()

      textData.forEach{(k, v) ->
        outputStream.write(twoHyphens)
        outputStream.write(boundary.toByteArray())
        outputStream.write(lineEnd)
        outputStream.write("Content-Disposition: form-data; name=\"$k\"".toByteArray())
        outputStream.write(lineEnd)
        outputStream.write(v.toByteArray())
        outputStream.write(lineEnd)
      }
      files.forEach{(name, file) ->
        outputStream.write(twoHyphens)
        outputStream.write(boundary.toByteArray())
        outputStream.write(lineEnd)
        outputStream.write("Content-isposition: form-data; name=\"$name\"; filename=\"${file.fileName}\"".toByteArray())
        outputStream.write(lineEnd)
        outputStream.write("Content-Type: ${file.mimeType}".toByteArray())
        outputStream.write(lineEnd)
        outputStream.write(lineEnd)
        outputStream.write(file.bytes)
        outputStream.write(lineEnd)
      }
      outputStream.write(twoHyphens)
      outputStream.write(boundary.toByteArray())
      outputStream.write(twoHyphens)
      outputStream.write(lineEnd)

      val reqBody = outputStream.toByteArray()

      val headers = if(token.isNotEmpty()) {
        mapOf("Content-Type" to "multipart/form-data; boundary=$boundary", "Content-Length" to reqBody.size.toString(), "Authorization" to "Bearer $token")
      } else {
        mapOf("Content-Type" to "multipart/form-data; boundary=$boundary", "Content-Length" to reqBody.size.toString())
      }
      send(HttpReq(addr + route, String(reqBody), method, headers))
    }
  }
}