package nr.dev.ourwater2

import android.graphics.BitmapFactory
import android.net.Uri
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.pulltorefresh.PullToRefreshBox
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import nr.dev.ourwater2.ui.theme.OurWater2Theme
import java.text.NumberFormat
import java.time.format.DateTimeFormatter
import java.util.Locale

class SubmitConsumptionDebit: ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWater2Theme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var debit by remember { mutableStateOf("") }
                    var customerName by remember { mutableStateOf("") }
                    var customerAddress by remember { mutableStateOf("") }
                    var showDialog by remember { mutableStateOf(false) }
                    val scope = rememberCoroutineScope()
                    val ctx = LocalContext.current
                    var dropdownOpened by remember { mutableStateOf(false) }
                    val dropdownItems = remember { mutableStateListOf<Customer2>() }
                    var selectedCustomer by remember { mutableStateOf<Customer2?>(null) }
                    var submittedRecord by remember { mutableStateOf<SubmittedConsumptionDebit?>(null) }
//                    var item by remember { mutableStateOf<Bill?>(null) }

                    LaunchedEffect(Unit) {
//                        item = HttpClient.getBill(intent.getIntExtra("id", -1))
                    }

                    LaunchedEffect(selectedCustomer) {
                        if(selectedCustomer != null) {
                            submittedRecord = HttpClient.getSubmittedConsumptionDebit(selectedCustomer!!.id)
                        }
                    }

//                    LaunchedEffect(submittedRecord) {
//                        if(submi)
//                    }



                    if (HttpClient.user == null) return@Scaffold
                    Column(
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                            .padding(12.dp)
                    ) {
                        Row(
                            Modifier
                                .heightIn(max = 100.dp)
                                .fillMaxWidth()
                                .padding(bottom = 12.dp),
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            IconButton({
                                finish()
                            }) {
                                Icon(
                                    painterResource(R.drawable.arr_back),
                                    contentDescription = "Arrow Back",
                                )
                            }
                            Spacer(Modifier.width(12.dp))
                            Text(
                                "Submit Consumption Debit",
                                fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                fontWeight = FontWeight.Bold
                            )
                        }
                        LazyColumn(Modifier.weight(1f)) {
                            item {
                                OutlinedTextField(customerName, {customerName = it}, Modifier.fillMaxWidth(), label = {Text("Customer Name")})
                                DropdownMenu(dropdownOpened, {dropdownOpened = false}, Modifier.fillMaxWidth()) {
                                    dropdownItems.forEach { item ->
                                        DropdownMenuItem({
                                            Text("${item.name} - ${item.address}", overflow = TextOverflow.Ellipsis, maxLines = 1)
                                        }, {
                                            selectedCustomer = item
                                        })
                                    }
                                }
                                if(selectedCustomer != null) Text("Address : ${selectedCustomer!!.address}", Modifier.fillMaxWidth().padding(start = 12.dp))
                                Spacer(Modifier.height(12.dp))
                                OutlinedTextField(debit, {debit = it}, Modifier.fillMaxWidth(), label = {Text("Debit (M³)")})

                                if(submittedRecord != null) {
                                    TextButton({}) {
                                        Text("Detail")
                                    }
                                    if(showDialog) {
                                        Dialog({showDialog = false}) {
                                            Column(Modifier.fillMaxWidth()) {
                                                Text("Debit : ${"%.2f".format(submittedRecord!!.debit)} M³")
                                                Text("Status : ${submittedRecord!!.status}")
                                            }
                                        }
                                    }
                                }


                                if(selectedCustomer != null) Text("Address : ${selectedCustomer!!.address}")
                                OutlinedTextField(debit, {debit = it}, Modifier.fillMaxWidth())


                                var selectedImageBitmap by remember {
                                    mutableStateOf<ImageBitmap?>(
                                        null
                                    )
                                }
                                var imageUri by remember { mutableStateOf<Uri?>(null) }
                                val imagePickerLauncher = rememberLauncherForActivityResult(
                                    PickJpgPngContract()
                                ) { uri: Uri? ->
                                    uri?.let {
                                        imageUri = it
                                        try {
                                            contentResolver.openInputStream(it)?.use { stream ->
                                                val bytes =
                                                    stream.buffered().use { it.readBytes() }
                                                selectedImageBitmap =
                                                    BitmapFactory.decodeByteArray(
                                                        bytes,
                                                        0,
                                                        bytes.size
                                                    ).asImageBitmap()
                                            }
                                        } catch (e: Exception) {
                                            e.printStackTrace()
                                            selectedImageBitmap = null
                                        }
                                    }
                                }
                                Text("Payment Proof")
                                if (selectedImageBitmap != null) {
                                    Image(
                                        selectedImageBitmap!!,
                                        modifier = Modifier
                                            .fillMaxWidth()
                                            .padding(12.dp),
                                        contentDescription = ""
                                    )
                                } else {
                                    Box(
                                        Modifier
                                            .fillMaxWidth()
                                            .background(Color.LightGray)
                                            .padding(24.dp)
                                    ) {
                                        Text("Not Paid Yet")
                                    }
                                }
//                                if (item!!.status in listOf("Pending", "Rejected")) {
//                                    Spacer(Modifier.height(12.dp))
//                                    OutlinedButton({
//                                        imagePickerLauncher.launch("image/*")
//                                    }, shape = RoundedCornerShape(16.dp)) {
//                                        Text("Pick Image")
//                                    }
//                                    Spacer(Modifier.height(12.dp))
//                                    Button({}, Modifier.fillMaxWidth()) {
//                                        Text("Save")
//                                    }
//                                }

                            }
                        }
                    }
                }
            }
        }
    }
}