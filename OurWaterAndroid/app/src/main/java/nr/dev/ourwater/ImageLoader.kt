package nr.dev.ourwater

import androidx.compose.animation.animateColor
import androidx.compose.animation.core.RepeatMode
import androidx.compose.animation.core.infiniteRepeatable
import androidx.compose.animation.core.rememberInfiniteTransition
import androidx.compose.animation.core.tween
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.layout.ContentScale

class ImageLoader {
    val caches = mutableMapOf<String, ImageBitmap>()

    suspend fun loadImg(path: String): ImageBitmap? {
        caches[path]?.let { return it }
        val img = HttpClient.fetchImg(path)
        if (img != null) caches[path] = img
        return img
    }

    fun hasCached(path: String): Boolean {
        return caches.contains(path)
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
    var loading by remember { mutableStateOf(false) }
    var error by remember { mutableStateOf(false) }
    var img by remember { mutableStateOf<ImageBitmap?>(null) }

    LaunchedEffect(url) {
        if (!imgLoader.hasCached(url)) loading = true
        error = false
        img = imgLoader.loadImg(url)
        if(img == null) error = true
        loading = false
    }

    when {
        loading -> {
            val trans = rememberInfiniteTransition()
            val bg by trans.animateColor(
                Color.White, Color.LightGray, infiniteRepeatable(
                    tween(300),
                    RepeatMode.Reverse
                )
            )

            Box(modifier.fillMaxSize().background(bg))
        }
        error -> {
            Box(modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                Text("Failed to load image")
            }
        }
        img != null -> {
            Image(bitmap = img!!, modifier = modifier, contentDescription = contentDescription, contentScale = contentScale)
        }
    }
}