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
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import nr.dev.ourwater.ui.theme.OurWaterTheme
import java.time.format.DateTimeFormatter

class ConsumptionDebitDetailActivity: ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            OurWaterTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var reason by remember { mutableStateOf("") }
                    val ctx = LocalContext.current
                    var rec by remember { mutableStateOf<ConsumptionDebit?>(null) }
                    val scope = rememberCoroutineScope()
                    val role = HttpClient.role
                    var errMsg by remember { mutableStateOf("") }

                    LaunchedEffect(Unit) {
                        rec = HttpClient.getConsumptionDebitById(intent.getIntExtra("id", 0))
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
                        if(rec == null) return@Column
                        LazyColumn(Modifier.weight(1f).padding(top = 24.dp)) {
                            item {
                                Text("${rec!!.date.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"))} - ${rec!!.status}", fontSize = MaterialTheme.typography.titleLarge.fontSize, fontWeight = FontWeight.Bold)
                                Spacer(Modifier.height(24.dp))
                                Text("Debit : %.2f M³".format(rec!!.debit))
                                Text("Inputted By : ${if(rec!!.inputtedBy == HttpClient.user?.fullName) "You" else rec!!.inputtedBy}")
                                if(role != "customer") Text("Customer : ${rec!!.customerName}")
                                if(!rec!!.correctedBy.isNullOrEmpty()) Text("Corrected By : ${rec!!.correctedBy}")
                                Spacer(Modifier.height(12.dp))
                                Text("Image")
                                NetImage(rec!!.imagePath, modifier = Modifier.fillMaxWidth())
                                Spacer(Modifier.height(40.dp))
                                Text("Rejection Reason")
                                OutlinedTextField(reason, {reason = it}, singleLine = false, maxLines = 5, modifier = Modifier.fillMaxWidth(), readOnly = role == "customer" || rec!!.status == "Pending")
                                Spacer(Modifier.height(12.dp))
                                if(role == "customer") return@item
                                if(rec!!.status != "Pending") return@item
                                ErrText(errMsg, Modifier.fillMaxWidth())
                                Row(Modifier.fillMaxWidth().padding(vertical = 24.dp), verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.SpaceBetween) {
                                    Button({
                                        if(reason.isBlank()) {
                                            errMsg = "Rejection Reason required"
                                            return@Button
                                        }
                                        scope.launch {
                                            HttpClient.patchConsumptionDebit(rec!!.id, reason, "Rejected")
                                        }
                                    }) {
                                        Text("Reject")
                                    }
                                    Button({
                                        scope.launch {
                                            HttpClient.patchConsumptionDebit(rec!!.id, "", "Verified")
                                        }
                                    }) {
                                        Text("Verify")
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