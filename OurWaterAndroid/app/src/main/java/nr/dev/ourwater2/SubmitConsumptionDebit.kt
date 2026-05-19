package nr.dev.ourwater2

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
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import nr.dev.ourwater2.ui.theme.OurWater2Theme

class SubmitConsumptionDebit : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWater2Theme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var debit by remember { mutableStateOf("") }
                    var customerName by remember { mutableStateOf("") }
                    var errMsg by remember { mutableStateOf("") }
                    var showDialog by remember { mutableStateOf(false) }
                    var loading by remember { mutableStateOf(false) }
                    val scope = rememberCoroutineScope()
                    val customerId = intent.getIntExtra("customerId", -1)
                    var dropdownOpened by remember { mutableStateOf(false) }
                    val dropdownItems = remember { mutableStateListOf<Customer2>() }
                    var selectedCustomer by remember { mutableStateOf<Customer2?>(null) }
                    var submittedRecord by remember {
                        mutableStateOf<SubmittedConsumptionDebit?>(
                            null
                        )
                    }

                    LaunchedEffect(selectedCustomer) {
                        if (selectedCustomer != null) {
                            submittedRecord =
                                HttpClient.getSubmittedConsumptionDebit(selectedCustomer!!.id)
                        }
                    }

                    LaunchedEffect(customerName) {
                        delay(500)
                        dropdownItems.clear()
                        dropdownItems.addAll(HttpClient.getCustomers(customerName))
                        dropdownOpened = dropdownItems.isNotEmpty()
                    }

                    LaunchedEffect(submittedRecord) {
                        if(submittedRecord == null) return@LaunchedEffect
                        debit = submittedRecord!!.debit.toString()
                    }


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
                                if (HttpClient.user!!.role == "officer") {

                                    OutlinedTextField(
                                        customerName,
                                        { customerName = it },
                                        Modifier.fillMaxWidth(),
                                        label = { Text("Customer Name") })
                                    Box(Modifier.fillMaxWidth()) {
                                        DropdownMenu(dropdownOpened, { dropdownOpened = false }) {
                                            dropdownItems.forEach { item ->
                                                DropdownMenuItem({
                                                    Text(
                                                        "${item.name} - ${item.phoneNumber}",
                                                        overflow = TextOverflow.Ellipsis,
                                                        maxLines = 1
                                                    )
                                                }, {
                                                    selectedCustomer = item
                                                    customerName = item.name
                                                    dropdownOpened = false
                                                })
                                            }
                                        }
                                    }
                                    if (selectedCustomer != null) Text(
                                        "Address : ${selectedCustomer!!.address}",
                                        Modifier
                                            .fillMaxWidth()
                                            .padding(start = 12.dp)
                                    )
                                    if (selectedCustomer != null) Text(
                                        "Phone : ${selectedCustomer!!.phoneNumber}",
                                        Modifier
                                            .fillMaxWidth()
                                            .padding(start = 12.dp)
                                    )
                                }
                                Spacer(Modifier.height(12.dp))
                                OutlinedTextField(
                                    debit,
                                    { debit = it },
                                    Modifier.fillMaxWidth(),
                                    label = { Text("Debit (M³)") })

                                if (submittedRecord != null) {
                                    TextButton({showDialog = true}) {
                                        Text("Detail")
                                    }
                                    if (showDialog) {
                                        Dialog({ showDialog = false }) {
                                            Column(
                                                Modifier
                                                    .fillMaxWidth()
                                                    .clip(RoundedCornerShape(24.dp))
                                                    .background(Color.White)
                                                    .padding(24.dp)
                                            ) {
                                                Text("Debit : ${"%.2f".format(submittedRecord!!.debit)} M³")
                                                Text("Status : ${submittedRecord!!.status}")
                                                if (submittedRecord!!.rejectionReason.isNotEmpty()) Text(
                                                    "Rejection Reason : ${submittedRecord!!.rejectionReason}"
                                                )
                                            }
                                        }
                                    }
                                }


                                var selectedImageBitmap by remember {
                                    mutableStateOf<ImageBitmap?>(
                                        null
                                    )
                                }
                                var imageUri by remember { mutableStateOf<Uri?>(null) }
                                val imagePickerLauncher = rememberLauncherForActivityResult(
                                    PickJpgPngContract()
                                ) { uri: Uri? ->
                                    uri?.let { u ->
                                        imageUri = u
                                        scope.launch {
                                            selectedImageBitmap = contentResolver.AsImage(u)
                                        }
                                    }
                                }
                                Text("Proof")
                                if (selectedImageBitmap != null) {
                                    Image(
                                        selectedImageBitmap!!,
                                        modifier = Modifier
                                            .fillMaxWidth()
                                            .padding(12.dp),
                                        contentDescription = ""
                                    )
                                } else {
                                    if (submittedRecord != null && submittedRecord!!.imagePath != null) {
                                        NetImage(
                                            submittedRecord!!.imagePath!!,
                                            Modifier
                                                .fillMaxWidth()
                                                .padding(24.dp)
                                        )
                                    } else {
                                        Box(
                                            Modifier
                                                .fillMaxWidth()
                                                .background(Color.LightGray)
                                                .padding(24.dp)
                                        ) {
                                            Text("Not Image Selected")
                                        }
                                    }
                                }
                                ErrText(errMsg, Modifier.fillMaxWidth())
                                if (submittedRecord != null && submittedRecord!!.status == "Verified") return@item
                                Spacer(Modifier.height(12.dp))
                                OutlinedButton({
                                    imagePickerLauncher.launch("image/*")
                                }, shape = RoundedCornerShape(16.dp)) {
                                    Text("Pick Image")
                                }
                                Spacer(Modifier.height(12.dp))
                                Button({
                                    if (selectedCustomer == null) {
                                        errMsg = "Customer not selected"
                                        return@Button
                                    }
                                    if (debit.toDoubleOrNull() == null) {
                                        errMsg = "Debit not valid"
                                        return@Button
                                    }
                                    if (imageUri == null) {
                                        errMsg = "Proof image required"
                                        return@Button
                                    }
                                    errMsg = ""
                                    scope.launch {
                                        loading = true
                                        val bytes = contentResolver.getBytes(imageUri!!)
                                        if (bytes == null) {
                                            errMsg = "Failed to process selected file"
                                            return@launch
                                        }
                                        when (val msg = HttpClient.submitConsumptionDebit(
                                            if (customerId > 0) customerId else selectedCustomer!!.id,
                                            debit,
                                            File(
                                                contentResolver.getFilename(imageUri!!),
                                                bytes,
                                                contentResolver.getMimeType(imageUri!!),
                                            )
                                        )) {
                                            "ok" -> finish()
                                            else -> errMsg = msg
                                        }
                                        loading = false
                                    }
                                }, Modifier.fillMaxWidth(), enabled = !loading) {
                                    LoadingOrContent(
                                        loading,
                                        { Text(if (submittedRecord != null) "Save" else "Submit") })
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}