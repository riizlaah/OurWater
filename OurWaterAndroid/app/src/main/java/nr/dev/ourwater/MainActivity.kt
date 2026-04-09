package nr.dev.ourwater

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicSecureTextField
import androidx.compose.foundation.text.input.TextFieldState
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import nr.dev.ourwater.ui.theme.OurWaterTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        HttpClient.sharedPrefs = getSharedPreferences("app_prefs", MODE_PRIVATE)
        HttpClient.loadToken()
        enableEdgeToEdge()
        setContent {
            OurWaterTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var username by remember { mutableStateOf("") }
                    val pass = remember { TextFieldState() }
                    var loading by remember { mutableStateOf(false) }
                    val ctx = LocalContext.current
                    val scope = rememberCoroutineScope()
                    var errMsg by remember { mutableStateOf("") }

                    LaunchedEffect(Unit) {
                        if(HttpClient.me()) {
                            val intent = Intent(ctx, HomeActivity::class.java)
                            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                            ctx.startActivity(intent)
                        }
                    }

                    Column(Modifier
                        .fillMaxSize()
                        .padding(innerPadding)
                        .padding(12.dp)) {
                        Text(
                            "OurWater - Login",
                            fontSize = MaterialTheme.typography.headlineLarge.fontSize,
                            fontWeight = FontWeight.Bold
                        )
                        Spacer(Modifier.height(48.dp))
                        Text("Username")
                        OutlinedTextField(
                            username,
                            { username = it },
                            singleLine = true,
                            modifier = Modifier.fillMaxWidth()
                        )
                        Spacer(Modifier.height(12.dp))
                        Text("Password")
                        BasicSecureTextField(
                            pass,
                            modifier = Modifier
                                .fillMaxWidth()
                                .border(1.dp, MaterialTheme.colorScheme.secondary,
                                    RoundedCornerShape(4.dp)
                                )
                                .padding(16.dp)
                        )
                        Spacer(Modifier.height(24.dp))
                        ErrText(errMsg, Modifier.fillMaxWidth().padding(vertical = 8.dp))
                        Button({
                            if(username.isBlank()) {
                                errMsg = "Username is required"
                                return@Button
                            }
                            if(pass.text.isEmpty()) {
                                errMsg = "Password is required"
                                return@Button
                            }
                            errMsg = ""
                            scope.launch {
                                loading = true
                                when(val msg = HttpClient.login(username, pass.text.toString())) {
                                    "ok" -> {
                                        val intent = Intent(ctx, HomeActivity::class.java)
                                        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                                        ctx.startActivity(intent)
                                    }
                                    else -> errMsg = msg
                                }
                                loading = false
                            }
                        }, modifier = Modifier.fillMaxWidth()) {
                            if(loading) {
                                CircularProgressIndicator(Modifier.size(32.dp), color = Color.White)
                                return@Button
                            }
                            Text("Login")
                        }
                    }
                }
            }
        }
    }
}

@Composable
fun ErrText(msg: String, modifier: Modifier = Modifier) {
    if(msg.isNotEmpty()) Text(msg, color = Color.Red, textAlign = TextAlign.Center, modifier = modifier)
}
