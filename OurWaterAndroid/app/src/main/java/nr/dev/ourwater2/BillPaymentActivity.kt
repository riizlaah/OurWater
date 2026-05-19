@file:OptIn(ExperimentalMaterial3Api::class)

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
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.pulltorefresh.PullToRefreshBox
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
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
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import nr.dev.ourwater2.ui.theme.OurWater2Theme
import java.text.NumberFormat
import java.time.format.DateTimeFormatter
import java.util.Locale

class BillPaymentActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWater2Theme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    val scope = rememberCoroutineScope()
                    var refreshing by remember { mutableStateOf(false) }
                    var errMsg by remember { mutableStateOf("") }
                    var loading by remember { mutableStateOf(false) }
                    val ctx = LocalContext.current
                    var item by remember { mutableStateOf<Bill?>(null) }

                    LaunchedEffect(Unit) {
                        item = HttpClient.getBill(intent.getIntExtra("id", -1))
                    }

                    LaunchedEffect(refreshing) {
                        if (refreshing) {
                            item = HttpClient.getBill(intent.getIntExtra("id", -1))
                            refreshing = false
                        }
                    }

                    if (HttpClient.user == null || item == null) return@Scaffold
                    PullToRefreshBox(
                        refreshing,
                        { refreshing = true },
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                    ) {
                        Column(
                            Modifier
                                .fillMaxSize()
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
                                    "Bill Detail",
                                    fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                    fontWeight = FontWeight.Bold
                                )
                            }
                            LazyColumn(Modifier.weight(1f)) {
                                item {
                                    Text(
                                        "${item!!.createdAt.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))} - (${item!!.status})",
                                        fontSize = MaterialTheme.typography.titleLarge.fontSize,
                                        fontWeight = FontWeight.Bold,
                                        color = MaterialTheme.colorScheme.primary
                                    )
                                    Spacer(Modifier.height(24.dp))
                                    val locale = Locale("in", "ID")
                                    val currencyFormat = NumberFormat.getCurrencyInstance(locale)
                                    Text("Consumption Debit : ${"%.2f".format(item!!.consumptionDebit.debit)} M³")
                                    Text("Base Amount : ${currencyFormat.format(item!!.originalAmount)}")
                                    if (item!!.extraFine > 0) {
                                        Text("Fine Amount : ${currencyFormat.format(item!!.extraFine)}")
                                        Column(Modifier.padding(12.dp)) {
                                            item!!.fines.forEach { Text(it, color = Color.Gray) }
                                        }
                                    }
                                    Text(
                                        "Total Amount : ${currencyFormat.format(item!!.totalAmount)}",
                                        fontWeight = FontWeight.SemiBold,
                                    )
                                    Spacer(Modifier.height(12.dp))
                                    Text(
                                        "Deadline : ${
                                            item!!.deadline.format(
                                                DateTimeFormatter.ofPattern(
                                                    "dd-MM-yyyy"
                                                )
                                            )
                                        }", color = Color(0xFFA20000), fontWeight = FontWeight.SemiBold
                                    )
                                    Spacer(Modifier.height(24.dp))

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
                                            scope.launch { selectedImageBitmap = contentResolver.AsImage(it) }
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
                                        if (item!!.imagePath != null) {
                                            NetImage(
                                                item!!.imagePath!!,
                                                Modifier
                                                    .fillMaxWidth()
                                                    .padding(12.dp)
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
                                    }
                                    if(item!!.rejectionReason.isNotEmpty()) {
                                        Text("Rejection Reason", fontWeight = FontWeight.SemiBold)
                                        Text(item!!.rejectionReason, Modifier.fillMaxWidth())
                                    }
                                    if (item!!.status in listOf("Pending", "Rejected")) {
                                        Spacer(Modifier.height(12.dp))
                                        OutlinedButton({
                                            imagePickerLauncher.launch("image/*")
                                        }, shape = RoundedCornerShape(16.dp)) {
                                            Text("Pick Image")
                                        }
                                        Spacer(Modifier.height(12.dp))
                                        ErrText(errMsg, Modifier.fillMaxWidth().padding(bottom = 12.dp))
                                        Button({
                                            if(imageUri == null) {
                                                errMsg = "Please select an image"
                                                return@Button
                                            }
                                            errMsg = ""
                                            scope.launch {
                                                loading = true
                                                val bytes = contentResolver.getBytes(imageUri!!)?: return@launch
                                                when(val msg = HttpClient.payBill(item!!.id, File(contentResolver.getFilename(imageUri!!), bytes, contentResolver.getMimeType(imageUri!!)))) {
                                                    "ok" -> refreshing = true
                                                    else -> errMsg = msg
                                                }
                                                loading = false
                                            }
                                        }, Modifier.fillMaxWidth(), enabled = !loading) {
                                            LoadingOrContent(loading, {Text("Save")})
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}