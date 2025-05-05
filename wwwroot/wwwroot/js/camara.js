// Variables globales para la cámara de registro
let videoElementRegistro;
let streamRegistro;

// Inicializar cámara de registro
async function initCamara(dotNetRef) {
    try {
        videoElementRegistro = document.getElementById('videoCamRegistro');
        if (streamRegistro) {
            streamRegistro.getTracks().forEach(track => track.stop());
        }

        streamRegistro = await navigator.mediaDevices.getUserMedia({
            video: { width: 320, height: 240, facingMode: 'user' },
            audio: false
        });

        videoElementRegistro.srcObject = streamRegistro;
    } catch (err) {
        dotNetRef.invokeMethodAsync('MostrarErrorCamara', err.message);
    }
}

// Capturar foto desde la cámara de registro
async function capturarFoto(dotNetRef) {
    if (!videoElementRegistro || !streamRegistro) {
        console.error("La cámara no está inicializada");
        return;
    }

    const canvas = document.createElement('canvas');
    canvas.width = videoElementRegistro.videoWidth;
    canvas.height = videoElementRegistro.videoHeight;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(videoElementRegistro, 0, 0, canvas.width, canvas.height);

    const imageData = canvas.toDataURL('image/jpeg', 0.8);
    await dotNetRef.invokeMethodAsync('GuardarRostro', imageData);
}

// Detener la cámara de registro
function detenerCamara() {
    if (streamRegistro) {
        streamRegistro.getTracks().forEach(track => track.stop());
        streamRegistro = null;
    }
    if (videoElementRegistro) {
        videoElementRegistro.srcObject = null;
    }
}

// Variables globales para la cámara de login
let videoElementLogin;
let streamLogin;

// Inicializar cámara de login
async function initCamaraLogin(dotNetRef) {
    try {
        videoElementLogin = document.getElementById('videoCamLogin');
        if (streamLogin) {
            streamLogin.getTracks().forEach(track => track.stop());
        }

        streamLogin = await navigator.mediaDevices.getUserMedia({
            video: { width: 320, height: 240, facingMode: 'user' },
            audio: false
        });

        videoElementLogin.srcObject = streamLogin;
    } catch (err) {
        dotNetRef.invokeMethodAsync('MostrarErrorCamara', err.message);
    }
}

// Reconocimiento facial para login
async function reconocerRostro(dotNetRef) {
    try {
        if (!videoElementLogin || !streamLogin) {
            throw new Error("La cámara no está inicializada");
        }

        const canvas = document.createElement('canvas');
        canvas.width = videoElementLogin.videoWidth;
        canvas.height = videoElementLogin.videoHeight;
        const ctx = canvas.getContext('2d');
        ctx.drawImage(videoElementLogin, 0, 0, canvas.width, canvas.height);

        const fotoActual = canvas.toDataURL('image/jpeg', 0.8);
        const rostrosRegistrados = await dotNetRef.invokeMethodAsync('ObtenerRostrosRegistrados');

        let coincidencia = false;
        for (const registro of rostrosRegistrados) {
            if (registro.FotoRostro && await compararImagenes(registro.FotoRostro, fotoActual)) {
                await dotNetRef.invokeMethodAsync('RostroReconocido', registro.Correo);
                coincidencia = true;
                break;
            }
        }

        if (!coincidencia) {
            await dotNetRef.invokeMethodAsync('RostroNoReconocido');
        }
    } catch (err) {
        dotNetRef.invokeMethodAsync('MostrarErrorCamara', err.message);
    }
}

// Comparar imágenes base64
async function compararImagenes(img1, img2) {
    if (!img1 || !img2) {
        return false;
    }

    const lengthToCompare = Math.min(100, img1.length, img2.length);
    return img1.substring(0, lengthToCompare) === img2.substring(0, lengthToCompare);
}

// Exponer métodos globalmente
window.initCamara = initCamara;
window.capturarFoto = capturarFoto;
window.detenerCamara = detenerCamara;
window.initCamaraLogin = initCamaraLogin;
window.reconocerRostro = reconocerRostro;