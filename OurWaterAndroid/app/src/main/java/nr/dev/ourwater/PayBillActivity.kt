package nr.dev.ourwater

import android.net.Uri
import android.os.Bundle
import android.widget.Space
import androidx.activity.ComponentActivity
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import nr.dev.ourwater.ui.theme.OurWaterTheme
import java.time.format.DateTimeFormatter

class PayBillActivity: ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWaterTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var loading by remember { mutableStateOf(false) }
                    val ctx = LocalContext.current
                    var bill by remember { mutableStateOf<Bill?>(null) }
                    val scope = rememberCoroutineScope()
                    var errMsg by remember { mutableStateOf("") }
                    var selectedImgUri by remember { mutableStateOf<Uri?>(null) }
                    val pickMedia = rememberLauncherForActivityResult(ActivityResultContracts.OpenDocument()) {
                        uri -> selectedImgUri = uri
                    }

                    LaunchedEffect(Unit) {
                        bill = HttpClient.getBillById(intent.getIntExtra("id", 0))
                    }

                    Column(
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                            .padding(12.dp)
                    ) {
                        Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
                            Icon(painterResource(R.drawable.arrow_back), contentDescription = "Back", modifier = Modifier.clickable(onClick = {
                                finish()
                            }))
                            Spacer(Modifier.width(12.dp))
                            Text(
                                "OurWater - Pay Bill",
                                fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                fontWeight = FontWeight.Bold
                            )
                        }
                        if(bill == null) return@Column
                        LazyColumn(Modifier.weight(1f).padding(top = 24.dp)) {
                            item {
                                Text("${bill!!.createdAt.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))} - ${bill!!.status}", fontSize = MaterialTheme.typography.titleLarge.fontSize, fontWeight = FontWeight.Bold)
                                Spacer(Modifier.height(24.dp))
                                Text("Debit : %.2f M³ at %s".format(bill!!.consumptionRecord.debit, bill!!.consumptionRecord.date.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))))
                                Text("${if(bill!!.fines.isEmpty()) "" else "Original "}Amount : Rp${bill!!.amount}")
                                if(bill!!.fines.isNotEmpty()) {
                                    bill!!.fines.forEach { t ->  Text(t) }
                                    Spacer(Modifier.height(12.dp))
                                    Text("Total Fines Amount : ${bill!!.fineAmount}")
                                    Text("Total Amount : ${bill!!.amount}")
                                }
                                Text("Deadline : %s".format(bill!!.deadline.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))))
                                Spacer(Modifier.height(12.dp))
                                if(bill!!.imagePath != null) {
                                    Text("Image")
                                    NetImage(bill!!.imagePath!!, modifier = Modifier.fillMaxWidth())
                                    Spacer(Modifier.height(12.dp))
                                }
                                if(bill!!.rejectionReason.isNotEmpty()) {
                                    Text("Rejection Reason")
                                    Text(bill!!.rejectionReason)
                                }
                                Spacer(Modifier.height(12.dp))
                                if(bill!!.status == "Approved" || bill!!.status == "Paid Unconfirmed") return@item
                                Text("Upload Invoice/Receipt")
                                OutlinedButton({
                                    pickMedia.launch(arrayOf("image/png", "image/jpg", "image/jpeg"))
                                }) {
                                    Text(
                                        if(selectedImgUri != null) {
                                            ctx.getFileName(selectedImgUri!!) ?: "Pick Media"
                                        } else "Pick Media"
                                    )
                                }
                                ErrText(errMsg, Modifier.fillMaxWidth().padding(vertical = 12.dp))
                                Spacer(Modifier.height(24.dp))
                                Button({
                                    if(selectedImgUri == null) {
                                        errMsg = "Evidence required"
                                        return@Button
                                    }
                                    errMsg = ""
                                    scope.launch {
                                        loading = true
                                        var bytes: ByteArray? = null
                                        contentResolver.openInputStream(selectedImgUri!!)?.use {
                                            bytes = it.readBytes()
                                        }
                                        if(bytes == null) {
                                            errMsg = "Error Parsing File"
                                            return@launch
                                        }
                                        val file = FileUpload(getFileName(selectedImgUri!!) ?: "default.jpg", bytes, getFileMimeType(selectedImgUri!!) ?: "application/octet-stream")
                                        when(val msg = HttpClient.submitPayment(file, bill!!.id)) {
                                            "ok" -> finish()
                                            else -> errMsg = msg
                                        }
                                        loading = false
                                    }
                                }, modifier = Modifier.fillMaxWidth()) {
                                    if(loading) {
                                        CircularProgressIndicator(Modifier.size(32.dp), color = Color.White)
                                        return@Button
                                    }
                                    Text("Submit")
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}