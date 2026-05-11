@file:OptIn(ExperimentalMaterial3Api::class)

package nr.dev.ourwater2

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
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
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
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
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import nr.dev.ourwater2.ui.theme.OurWater2Theme
import java.time.format.DateTimeFormatter

class ConsumptionDebitDetailActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        HttpClient.sharedPrefs = getSharedPreferences("prefs", MODE_PRIVATE)
        enableEdgeToEdge()
        setContent {
            OurWater2Theme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    val scope = rememberCoroutineScope()
                    var refreshing by remember { mutableStateOf(false) }
                    val ctx = LocalContext.current
                    var item by remember { mutableStateOf<ConsumptionDebit?>(null) }

                    LaunchedEffect(Unit) {
                        item = HttpClient.getConsumptionDebit(intent.getIntExtra("id", -1))
                    }

                    LaunchedEffect(refreshing) {
                        if (refreshing) {
                            item = HttpClient.getConsumptionDebit(intent.getIntExtra("id", -1))
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
                        Column(Modifier
                            .fillMaxSize()
                            .padding(horizontal = 12.dp)) {
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
                                    "Consumption Debit Detail",
                                    fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                    fontWeight = FontWeight.Bold
                                )
                            }
                            LazyColumn(Modifier.weight(1f)) {
                                item {
                                    Text(
                                        "${item!!.date.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))} - (${item!!.status})",
                                        fontSize = MaterialTheme.typography.titleLarge.fontSize,
                                        fontWeight = FontWeight.Bold
                                    )
                                    Spacer(Modifier.height(24.dp))
                                    if (HttpClient.user?.role != "customer") {
                                        Text("Customer Name : ${item!!.customerName}")
                                        Text("Location : ${item!!.location}")
                                    }
                                    Spacer(Modifier.height(12.dp))
                                    val inputtedBy =
                                        if (item!!.inputtedBy == HttpClient.user?.fullname) "You" else item!!.inputtedBy
                                    Text("Inputted By : $inputtedBy")
                                    if (item!!.correctedBy != null) {
                                        val correctedBy =
                                            if (item!!.correctedBy == HttpClient.user?.fullname) "You" else item!!.correctedBy
                                        Text("Corrected By : $correctedBy")
                                    }
                                    Text("Debit : ${"%.2f".format(item!!.debit)} M³")
                                    if (item!!.prevDebit != null) Text(
                                        "Previous Debit : ${
                                            "%.2f".format(
                                                item!!.prevDebit
                                            )
                                        } M³"
                                    )
                                    Spacer(Modifier.height(12.dp))
                                    Text("Proof", fontWeight = FontWeight.SemiBold)
                                    NetImage(
                                        item!!.imagePath,
                                        Modifier
                                            .heightIn(min = 200.dp)
                                            .fillMaxWidth()
                                            .padding(12.dp)
                                    )
                                    if (item!!.rejectionReason.isNotEmpty()) {
                                        Text("Rejection reason", fontWeight = FontWeight.SemiBold)
                                        Text(
                                            item!!.rejectionReason,
                                            modifier = Modifier
                                                .fillMaxWidth()
                                                .padding(horizontal = 12.dp)
                                        )
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