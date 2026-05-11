package nr.dev.ourwater2

import androidx.compose.animation.animateColor
import androidx.compose.animation.core.RepeatMode
import androidx.compose.animation.core.infiniteRepeatable
import androidx.compose.animation.core.rememberInfiniteTransition
import androidx.compose.animation.core.tween
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.layout.ContentScale

class ImageLoader {
    val caches = mutableMapOf<String, ImageBitmap>()

    suspend fun fetchImg(url: String): ImageBitmap? {
        caches[url]?.let { return it }
        val img = HttpClient.fetchImg(url)
        if (img != null) caches[url] = img
        return img
    }

    fun has(url: String): Boolean {
        return caches.contains(url)
    }
}

@Composable
fun NetImage(
    url: String,
    modifier: Modifier = Modifier,
    contentDescription: String = "",
    contentScale: ContentScale = ContentScale.Fit
) {
    val imgLoader = remember { ImageLoader() }
    var img by remember { mutableStateOf<ImageBitmap?>(null) }
    var loading by remember { mutableStateOf(false) }
    var error by remember { mutableStateOf(false) }

    LaunchedEffect(url) {
        error = false
        if (!imgLoader.has(url)) loading = true
        img = imgLoader.fetchImg(url)
        if (img == null) error = true
        loading = false
    }

    when {
        loading -> {
            val infTrans = rememberInfiniteTransition()
            val bgColor by infTrans.animateColor(
                Color.LightGray, Color.Gray, infiniteRepeatable(
                    tween(500),
                    repeatMode = RepeatMode.Reverse
                )
            )
            Box(modifier.background(bgColor))
        }
        error -> {
            Box(modifier) {
                Text("Image failed to load")
            }
        }
        img != null -> {
            Image(img!!, contentDescription = contentDescription, modifier = modifier, contentScale = contentScale)
        }
    }
}