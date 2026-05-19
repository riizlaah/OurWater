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
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
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
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.rotate
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import nr.dev.ourwater2.ui.theme.OurWater2Theme
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class HomeActivity : ComponentActivity() {
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
                    val consumptionDebits = remember { mutableStateListOf<ConsumptionDebit>() }

                    LaunchedEffect(Unit) {
                        consumptionDebits.clear()
                        consumptionDebits.addAll(HttpClient.getConsumptionDebits())
                    }

                    LaunchedEffect(refreshing) {
                        if (refreshing) {
                            consumptionDebits.clear()
                            consumptionDebits.addAll(HttpClient.getConsumptionDebits())
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
                                    .padding(bottom = 12.dp),
                                verticalAlignment = Alignment.CenterVertically
                            ) {
                                IconButton({
                                    HttpClient.token = ""
                                    HttpClient.saveToken()
                                    val intent = Intent(ctx, MainActivity::class.java)
                                    intent.flags =
                                        Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                                    startActivity(intent)
                                }) {
                                    Icon(
                                        painterResource(R.drawable.logout),
                                        contentDescription = "Log out",
                                        modifier = Modifier.rotate(180f),
                                        tint = Color.Red
                                    )
                                }
                                Spacer(Modifier.width(12.dp))
                                Text(
                                    "Hello ${HttpClient.user?.fullname ?: "User"}!",
                                    fontSize = MaterialTheme.typography.headlineLarge.fontSize,
                                    fontWeight = FontWeight.Bold
                                )
                            }
                            Row(
                                Modifier
                                    .fillMaxWidth()
                                    .padding(bottom = 24.dp),
                                horizontalArrangement = Arrangement.spacedBy(12.dp)
                            ) {
                                if(allowedSubmitDays.contains(LocalDate.now().dayOfMonth)) {
                                    Button(
                                        {
                                            val intent = Intent(ctx, SubmitConsumptionDebit::class.java)
                                            startActivity(intent)
                                        },
                                        modifier = Modifier.weight(1f),
                                        shape = RoundedCornerShape(12.dp)
                                    ) {
                                        Text("Submit Consumption Debit")
                                    }
                                }
                                if (HttpClient.user?.role == "customer") {
                                    OutlinedButton(
                                        {
                                            val intent = Intent(ctx, ViewBillsActivity::class.java)
                                            startActivity(intent)
                                        },
                                        modifier = Modifier.weight(1f),
                                        shape = RoundedCornerShape(12.dp)
                                    ) {
                                        Text("Bills")
                                    }
                                }
                            }
                            Text(
                                "Consumption Debit",
                                fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                                fontWeight = FontWeight.SemiBold
                            )
                            LazyColumn(Modifier.weight(1f)) {
                                items(consumptionDebits) { item ->
                                    Column(
                                        Modifier
                                            .padding(vertical = 12.dp)
                                            .fillMaxWidth()
                                            .clickable(onClick = {
                                                val intent = Intent(
                                                    ctx,
                                                    ConsumptionDebitDetailActivity::class.java
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
                                            "${item.date.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))} - (${item.status})",
                                            fontSize = MaterialTheme.typography.titleLarge.fontSize,
                                            fontWeight = FontWeight.Bold
                                        )
                                        Spacer(Modifier.height(24.dp))
                                        if (HttpClient.user?.role != "customer") {
                                            Text("Customer Name : ${item.customerName}")
                                            Text("Location : ${item.location}")
                                        }
                                        val inputtedBy =
                                            if (item.inputtedBy == HttpClient.user?.fullname) "You" else item.inputtedBy
                                        Text("Inputted By : ${inputtedBy}")
                                        Text("Debit : ${"%.2f".format(item.debit)} M³")
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