@file:OptIn(ExperimentalMaterial3Api::class)

package nr.dev.ourwater2

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.pulltorefresh.PullToRefreshBox
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import nr.dev.ourwater2.ui.theme.OurWater2Theme
import java.text.NumberFormat
import java.time.format.DateTimeFormatter
import java.util.Locale

class ViewBillsActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWater2Theme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    val scope = rememberCoroutineScope()
                    var refreshing by remember { mutableStateOf(false) }
                    val ctx = LocalContext.current
                    val allowedSubmitDays = (1..7) + (26..31)
                    val bills = remember { mutableStateListOf<Bill>() }

                    LaunchedEffect(Unit) {
                        bills.clear()
                        bills.addAll(HttpClient.getBills())
                    }

                    LaunchedEffect(refreshing) {
                        if (refreshing) {
                            bills.clear()
                            bills.addAll(HttpClient.getBills())
                            refreshing = false
                        }
                    }


                    if (HttpClient.user == null) return@Scaffold
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
                                    .fillMaxWidth()
                                    .padding(bottom = 24.dp),
                                horizontalArrangement = Arrangement.spacedBy(12.dp)
                            ) {
                                OutlinedButton(
                                    { finish() },
                                    modifier = Modifier.weight(1f),
                                    shape = RoundedCornerShape(12.dp)
                                ) {
                                    Text("Back")
                                }
                            }
                            Text(
                                "Bills",
                                fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                fontWeight = FontWeight.SemiBold
                            )
                            LazyColumn(Modifier.weight(1f)) {
                                items(bills) { item ->
                                    Column(
                                        Modifier
                                            .padding(vertical = 12.dp)
                                            .fillMaxWidth()
                                            .clickable(onClick = {
                                                val intent = Intent(
                                                    ctx,
                                                    BillPaymentActivity::class.java
                                                )
                                                intent.putExtra("id", item.id)
                                                startActivity(intent)
                                            })
                                            .shadow(3.dp, RoundedCornerShape(24.dp))
                                            .clip(RoundedCornerShape(24.dp))
                                            .background(Color.White)
                                            .padding(16.dp)
                                    ) {
                                        Text(
                                            "${item.createdAt.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))} - (${item.status})",
                                            fontSize = MaterialTheme.typography.titleLarge.fontSize,
                                            fontWeight = FontWeight.Bold
                                        )
                                        Spacer(Modifier.height(24.dp))
                                        val locale = Locale("in", "ID")
                                        val currencyFormat =
                                            NumberFormat.getCurrencyInstance(locale)
                                        Text("Consumption Debit : ${"%.2f".format(item.consumptionDebit.debit)} M³")
                                        Text("Base Amount : ${currencyFormat.format(item.originalAmount)}")
                                        if (item.extraFine > 0) {
                                            Text("Fine Amount : ${currencyFormat.format(item.extraFine)}")
                                        }
                                        Text(
                                            "Total Amount : ${currencyFormat.format(item.totalAmount)}",
                                            fontWeight = FontWeight.Bold
                                        )
                                        Text(
                                            "Deadline : ${
                                                item.deadline.format(
                                                    DateTimeFormatter.ofPattern(
                                                        "dd-MM-yyyy"
                                                    )
                                                )
                                            }"
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