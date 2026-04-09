package nr.dev.ourwater

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
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
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import nr.dev.ourwater.ui.theme.OurWaterTheme
import java.time.format.DateTimeFormatter

class HomeActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWaterTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    val ctx = LocalContext.current
                    val consDebRecords = remember { mutableStateListOf<ConsumptionDebit>() }
                    val scope = rememberCoroutineScope()
                    val role = HttpClient.role

                    LaunchedEffect(Unit) {
                        consDebRecords.addAll(HttpClient.getConsumptionDebitRecords())
                    }

                    Column(
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                            .padding(12.dp)
                    ) {
                        Text(
                            "OurWater - Home",
                            fontSize = MaterialTheme.typography.headlineSmall.fontSize,
                            fontWeight = FontWeight.Bold
                        )
                        Row(
                            Modifier
                                .fillMaxWidth()
                                .padding(12.dp),
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Button(
                                {
                                    val intent = Intent(ctx, SubmitConsumptionDebitRecordActivity::class.java)
                                    ctx.startActivity(intent)
                                },
                                shape = RoundedCornerShape(8.dp),
                                modifier = Modifier.weight(1f)
                            ) {
                                Text("Submit Consumption Debit")
                            }
                            if (role == "officer") return@Row
                            Spacer(Modifier.width(12.dp))
                            OutlinedButton(
                                {},
                                shape = RoundedCornerShape(8.dp),
                                modifier = Modifier.weight(1f)
                            ) {
                                Text("View Bills")
                            }
                        }
                        LazyColumn(Modifier.weight(1f)) {
                            items(consDebRecords) { rec ->
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
                                            val intent = Intent(ctx, ConsumptionDebitDetailActivity::class.java)
                                            intent.putExtra("id", rec.id)
                                            ctx.startActivity(intent)
                                        })
                                ) {
                                    Text("${rec.date.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))} - ${rec.status}", fontSize = MaterialTheme.typography.titleLarge.fontSize, fontWeight = FontWeight.Bold)
                                    Spacer(Modifier.height(24.dp))
                                    Text("Debit : %.2f M³".format(rec.debit))
                                    Text("Inputted By : ${if(rec.inputtedBy == HttpClient.user?.fullName) "You" else rec.inputtedBy}")
                                    if(role != "customer") Text("Customer : ${rec.customerName}")
                                    if(rec.correctedBy.isNullOrEmpty()) return@Column
                                    Text("Corrected By : ${rec.correctedBy}")
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}