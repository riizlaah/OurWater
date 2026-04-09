@file:OptIn(ExperimentalMaterial3Api::class)

package nr.dev.ourwater

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
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
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import nr.dev.ourwater.ui.theme.OurWaterTheme
import java.time.format.DateTimeFormatter

class ViewBillsActivity: ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWaterTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var reason by remember { mutableStateOf("") }
                    var isRefreshing by remember { mutableStateOf(false) }
                    val ctx = LocalContext.current
                    val bills = remember { mutableStateListOf<Bill>() }
                    val scope = rememberCoroutineScope()
                    val role = HttpClient.role
                    var errMsg by remember { mutableStateOf("") }

                    LaunchedEffect(Unit) {
                        bills.addAll(HttpClient.getBills())
                    }

                    LaunchedEffect(isRefreshing) {
                        if(isRefreshing) {
                            bills.clear()
                            bills.addAll(HttpClient.getBills())
                            isRefreshing = false
                        }
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
                                "OurWater - Bills",
                                fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                fontWeight = FontWeight.Bold
                            )
                        }
                        PullToRefreshBox(isRefreshing, {isRefreshing = true}, modifier = Modifier.weight(1f).padding(vertical = 12.dp)) {
                            LazyColumn(Modifier.fillMaxSize()) {
                                items(bills) { bill ->
                                    Column(
                                        Modifier
                                            .fillMaxWidth()
                                            .padding(vertical = 8.dp)
                                            .border(
                                                1.dp,
                                                MaterialTheme.colorScheme.primary,
                                                RoundedCornerShape(12.dp)
                                            )
                                            .padding(16.dp)
                                            .clickable(onClick = {
                                                val intent = Intent(
                                                    ctx,
                                                    PayBillActivity::class.java
                                                )
                                                intent.putExtra("id", bill.id)
                                                ctx.startActivity(intent)
                                            })
                                    ) {
                                        Text(
                                            "${bill.createdAt.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))} - ${bill.status}",
                                            fontSize = MaterialTheme.typography.titleLarge.fontSize,
                                            fontWeight = FontWeight.Bold
                                        )
                                        Spacer(Modifier.height(24.dp))
                                        Text("Debit : %.2f M³".format(bill.consumptionRecord.debit))
                                        Text("Amount : Rp%s".format(bill.amount))
                                        Text("Deadline : ${bill.deadline.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))}")
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