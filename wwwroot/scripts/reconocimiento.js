let videoElement = null;
let descriptorCapturado = null;

async function loadModels() {
    await faceapi.nets.ssdMobilenetv1.loadFromUri('/models');
    await faceapi.nets.faceLandmark68Net.loadFromUri('/models');
    await faceapi.nets.faceRecognitionNet.loadFromUri('/models');
}

window.initCamaraLogin = async function (dotnetHelper) {
    videoElement = document.getElementById('videoCamLogin');
    await loadModels();

    navigator.mediaDevices.getUserMedia({ video: true })
        .then(stream => {
            videoElement.srcObject = stream;
        })
        .catch(err => {
            dotnetHelper.invokeMethodAsync("MostrarErrorCamara", "No se pudo acceder a la cámara.");
        });
};

window.reconocerRostro = async function (dotnetHelper) {
    try {
        const deteccion = await faceapi.detectSingleFace(videoElement).withFaceLandmarks().withFaceDescriptor();

        if (!deteccion) {
            await dotnetHelper.invokeMethodAsync("RostroNoReconocido");
            return;
        }

        descriptorCapturado = deteccion.descriptor;

        const usuarios = await dotnetHelper.invokeMethodAsync("ObtenerRostrosRegistrados");
        for (const usuario of usuarios) {
            try {
                const valores = usuario.fotoRostro.split(',').map(Number);
                if (valores.length !== 128) {
                    console.warn("Descriptor inválido (longitud ≠ 128) para usuario:", usuario.correo);
                    continue;
                }

                const descriptorGuardado = new Float32Array(valores);
                const distancia = faceapi.euclideanDistance(descriptorCapturado, descriptorGuardado);

                if (distancia < 0.45) {
                    await dotnetHelper.invokeMethodAsync("RostroReconocido", usuario.correo);
                    return;
                }
            } catch (e) {
                console.error("Error al procesar descriptor de usuario:", usuario.correo, e);
            }
        }

        await dotnetHelper.invokeMethodAsync("RostroNoReconocido");
    } catch (error) {
        console.error("Error general en reconocimiento facial:", error);
        await dotnetHelper.invokeMethodAsync("MostrarErrorCamara", "Error al iniciar reconocimiento facial: " + error.message);
    }
};

window.capturarFotoYDescriptor = async function (dotnetHelper) {
    try {
        const deteccion = await faceapi.detectSingleFace(videoElement).withFaceLandmarks().withFaceDescriptor();

        if (!deteccion) {
            await dotnetHelper.invokeMethodAsync("MostrarErrorCamara", "No se detectó ningún rostro");
            return;
        }

        const descriptor = deteccion.descriptor;
        await dotnetHelper.invokeMethodAsync("GuardarRostro", descriptor.toString());
    } catch (error) {
        console.error("Error al capturar descriptor:", error);
        await dotnetHelper.invokeMethodAsync("MostrarErrorCamara", "Error al capturar rostro: " + error.message);
    }
};

window.initCamaraRegistro = async function (dotnetHelper) {
    videoElement = document.getElementById('videoCamRegistro');
    await loadModels();

    navigator.mediaDevices.getUserMedia({ video: true })
        .then(stream => {
            videoElement.srcObject = stream;
        })
        .catch(err => {
            dotnetHelper.invokeMethodAsync("MostrarErrorCamara", "No se pudo acceder a la cámara.");
        });
};
