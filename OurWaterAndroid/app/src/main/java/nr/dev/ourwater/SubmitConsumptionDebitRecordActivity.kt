package nr.dev.ourwater

import android.net.Uri
import android.os.Bundle
import android.util.Patterns
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
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateListOf
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
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import nr.dev.ourwater.ui.theme.OurWaterTheme
import java.time.format.DateTimeFormatter

class SubmitConsumptionDebitRecordActivity: ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWaterTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var debit by remember { mutableStateOf("") }
                    var query by remember { mutableStateOf("") }
                    var listOpened by remember { mutableStateOf(false) }
                    var selectedCustomerId by remember { mutableIntStateOf(0) }
                    val customers = remember { mutableStateListOf<Customer>() }
                    val ctx = LocalContext.current
//                    var rec by remember { mutableStateOf<ConsumptionDebit?>(null) }
                    val scope = rememberCoroutineScope()
                    var selectedImgUri by remember { mutableStateOf<Uri?>(null) }
                    val pickMedia = rememberLauncherForActivityResult(contract = ActivityResultContracts.OpenDocument()) {
                        uri -> selectedImgUri = uri
                    }
                    val role = HttpClient.role
                    var loading by remember { mutableStateOf(false) }
                    var errMsg by remember { mutableStateOf("") }

                    LaunchedEffect(Unit) {
                        if(role == "customer") selectedCustomerId = HttpClient.user!!.id
                    }

                    LaunchedEffect(query) {
                        delay(300)
                        if(query.isNotBlank()) listOpened = true
                        customers.clear()
                        customers.addAll(HttpClient.searchCustomers(query))
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
                                "OurWater - Consumption Debit Detail",
                                fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                fontWeight = FontWeight.Bold
                            )
                        }
                        LazyColumn(Modifier.weight(1f).padding(top = 24.dp)) {
                            item {
                                if(role == "officer") {
                                    Text("Customer Id")
                                    Column(Modifier.fillMaxWidth()) {
                                        OutlinedTextField(query, {query = it}, singleLine = true, modifier = Modifier.fillMaxWidth())
                                        DropdownMenu(listOpened && customers.isNotEmpty(), {listOpened = false}) {
                                            customers.forEach { cust ->
                                                DropdownMenuItem({Text("${cust.id} - ${cust.name}")}, {selectedCustomerId = cust.id; listOpened = false; query = cust.id.toString()})
                                            }
                                        }
                                    }
                                    Spacer(Modifier.height(12.dp))
                                }
                                Text("Debit")
                                OutlinedTextField(debit, {debit = it}, singleLine = true, modifier = Modifier.fillMaxWidth(), keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Decimal))
                                Spacer(Modifier.height(12.dp))
                                Text("Evidence")
                                OutlinedButton({
                                    pickMedia.launch(arrayOf("image/png", "image/jpg", "image/jpeg"))
                                }) {
                                    Text(
                                        if(selectedImgUri != null) {
                                            ctx.getFileName(selectedImgUri!!) ?: "Pick Media"
                                        } else "Pick Media"
                                    )
                                }
                                ErrText(errMsg, Modifier.fillMaxWidth())
                                Spacer(Modifier.height(24.dp))
                                Button({
                                    query = selectedCustomerId.toString()
                                    if(selectedCustomerId == 0) {
                                        errMsg = "Customer id not valid"
                                        return@Button
                                    }
                                    val deb = debit.toDoubleOrNull()
                                    if(deb == null) {
                                        errMsg = "Debit not valid"
                                        return@Button
                                    }
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
                                        when(val msg = HttpClient.submitConsumptionDebit(file, selectedCustomerId, deb)) {
                                            "ok" -> finish()
                                            else -> errMsg = msg
                                        }
                                        loading = false
                                    }
                                }) {
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