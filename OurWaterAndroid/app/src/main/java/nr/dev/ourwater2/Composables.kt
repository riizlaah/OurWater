package nr.dev.ourwater2

import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.size
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp

@Composable
fun ErrText(msg: String, modifier: Modifier = Modifier) {
    if(msg.isNotEmpty()) Text(msg, color = Color.Red, modifier = Modifier.fillMaxWidth(), textAlign = TextAlign.Center)
}

@Composable
fun LoadingOrContent(loading: Boolean, content: @Composable () -> Unit, color: Color = Color.White, size: Dp = 24.dp) {
    if(loading) CircularProgressIndicator(color = color, modifier = Modifier.size(size))
    else content()
}