package nr.dev.ourwater2

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import nr.dev.ourwater2.ui.theme.OurWater2Theme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        HttpClient.sharedPrefs = getSharedPreferences("prefs", MODE_PRIVATE)
        HttpClient.loadToken()
        enableEdgeToEdge()
        setContent {
            OurWater2Theme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    var username by remember { mutableStateOf("") }
                    var password by remember { mutableStateOf("") }
                    val scope = rememberCoroutineScope()
                    var loading by remember { mutableStateOf(false) }
                    var errMsg by remember { mutableStateOf("") }
                    val ctx = LocalContext.current
                    LaunchedEffect(Unit) {
                        if(HttpClient.me()) {
                            val intent = Intent(ctx, HomeActivity::class.java)
                            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                            startActivity(intent)
                        }
                    }
                    Column(Modifier
                        .padding(innerPadding)
                        .padding(12.dp)) {
                        Text("Login", fontSize = MaterialTheme.typography.displayMedium.fontSize, fontWeight = FontWeight.Bold)
                        Spacer(Modifier.height(48.dp))
                        OutlinedTextField(username, {username = it}, modifier = Modifier.fillMaxWidth(), label = {Text("Username")})
                        Spacer(Modifier.height(12.dp))
                        OutlinedTextField(password, {password = it}, modifier = Modifier.fillMaxWidth(), label = {Text("Password")}, visualTransformation = PasswordVisualTransformation())
                        Spacer(Modifier.height(30.dp))
                        ErrText(errMsg)
                        Spacer(Modifier.height(12.dp))
                        Button({
                            if(username.isBlank()) {
                                errMsg = "Username is required"
                                return@Button
                            }
                            if(password.isEmpty()) {
                                errMsg = "Password is required"
                                return@Button
                            }
                            errMsg = ""
                            scope.launch {
                                loading = true
                                when(val msg = HttpClient.login(username, password)) {
                                    "ok" -> {
                                        val intent = Intent(ctx, HomeActivity::class.java)
                                        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                                        startActivity(intent)
                                    }
                                    else -> errMsg = msg
                                }
                                loading = false
                            }
                        }, modifier = Modifier.fillMaxWidth(), shape = RoundedCornerShape(8.dp)) {
                            LoadingOrContent(loading, {Text("Login")})
                        }
                    }
                }
            }
        }
    }
}

