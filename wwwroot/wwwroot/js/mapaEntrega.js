window.mostrarRutaEntrega = async (latTienda, lngTienda, latCliente, lngCliente) => {
    const mapaDiv = document.getElementById("mapa-entrega");
    
    try {
        // Limpiar mapa existente
        if (window.entregaMap) {
            window.entregaMap.remove();
        }
        mapaDiv.innerHTML = "";

        // Crear mapa centrado en el punto medio
        const centerLat = (latTienda + latCliente) / 2;
        const centerLng = (lngTienda + lngCliente) / 2;
        
        window.entregaMap = L.map(mapaDiv).setView([centerLat, centerLng], 13);

        // Capa base (OpenStreetMap)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
            maxZoom: 18
        }).addTo(window.entregaMap);

        // Iconos personalizados
        const tiendaIcon = L.icon({
            iconUrl: 'https://cdn-icons-png.flaticon.com/512/2776/2776000.png',
            iconSize: [32, 32],
            iconAnchor: [16, 32]
        });

        const clienteIcon = L.icon({
            iconUrl: 'https://cdn-icons-png.flaticon.com/512/447/447031.png',
            iconSize: [32, 32],
            iconAnchor: [16, 32]
        });

        // Marcador de la tienda
        const tiendaMarker = L.marker([latTienda, lngTienda], { icon: tiendaIcon })
            .addTo(window.entregaMap)
            .bindPopup("<b>Tienda</b><br>Punto de partida")
            .openPopup();

        // Marcador del cliente
        const clienteMarker = L.marker([latCliente, lngCliente], { icon: clienteIcon })
            .addTo(window.entregaMap)
            .bindPopup("<b>Cliente</b><br>Punto de entrega");

        // Obtener la ruta usando OSRM
        const response = await fetch(`https://router.project-osrm.org/route/v1/driving/${lngTienda},${latTienda};${lngCliente},${latCliente}?overview=full&geometries=geojson`);
        
        if (!response.ok) {
            throw new Error('Error al obtener la ruta');
        }

        const data = await response.json();

        if (data.routes && data.routes.length > 0) {
            const coordinates = data.routes[0].geometry.coordinates;
            const routeCoordinates = coordinates.map(coord => [coord[1], coord[0]]); // Convertir [lng, lat] a [lat, lng]

            // Dibujar la ruta real
            const ruta = L.polyline(routeCoordinates, {
                color: '#3498db',
                weight: 5,
                opacity: 0.7,
                lineJoin: 'round'
            }).addTo(window.entregaMap);

            // Añadir información de la ruta
            const distancia = (data.routes[0].distance / 1000).toFixed(1);
            const duracion = Math.round(data.routes[0].duration / 60);
            
            const infoRuta = L.control({position: 'bottomright'});
            infoRuta.onAdd = function(map) {
                const div = L.DomUtil.create('div', 'info-ruta');
                div.innerHTML = `
                    <div style="background: white; padding: 10px; border-radius: 5px; box-shadow: 0 0 10px rgba(0,0,0,0.1);">
                        <h4 style="margin: 0 0 5px 0;">Información de la Ruta</h4>
                        <p style="margin: 5px 0;">Distancia: ${distancia} km</p>
                        <p style="margin: 5px 0;">Duración estimada: ${duracion} minutos</p>
                    </div>
                `;
                return div;
            };
            infoRuta.addTo(window.entregaMap);

            // Ajustar el zoom para mostrar toda la ruta
            window.entregaMap.fitBounds(ruta.getBounds(), { padding: [50, 50] });
        } else {
            // Si no se puede obtener la ruta, mostrar una línea recta
            const ruta = L.polyline(
                [[latTienda, lngTienda], [latCliente, lngCliente]],
                {
                    color: '#3498db',
                    weight: 5,
                    opacity: 0.7,
                    dashArray: '10, 10',
                    lineJoin: 'round'
                }
            ).addTo(window.entregaMap);
            
            window.entregaMap.fitBounds(ruta.getBounds(), { padding: [50, 50] });
        }

        // Añadir control de escala
        L.control.scale().addTo(window.entregaMap);

        // Añadir control de zoom
        L.control.zoom({
            position: 'topright'
        }).addTo(window.entregaMap);

    } catch (error) {
        console.error("Error al mostrar el mapa:", error);
        // Si hay un error, mostrar una línea recta como fallback
        const ruta = L.polyline(
            [[latTienda, lngTienda], [latCliente, lngCliente]],
            {
                color: '#3498db',
                weight: 5,
                opacity: 0.7,
                dashArray: '10, 10',
                lineJoin: 'round'
            }
        ).addTo(window.entregaMap);
        
        window.entregaMap.fitBounds(ruta.getBounds(), { padding: [50, 50] });
    }
};

window.mostrarMapaEntregaRuta = function (mapId, latDelivery, lngDelivery, latCliente, lngCliente) {
    const mapDiv = document.getElementById(mapId);
    mapDiv.innerHTML = ""; // Limpia cualquier contenido previo

    const map = L.map(mapId).setView([latCliente, lngCliente], 13);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: "Leaflet"
    }).addTo(map);

    const markerDelivery = L.marker([latDelivery, lngDelivery]).addTo(map).bindPopup("Delivery").openPopup();
    const markerCliente = L.marker([latCliente, lngCliente]).addTo(map).bindPopup("Cliente");

    const ruta = L.polyline([
        [latDelivery, lngDelivery],
        [latCliente, lngCliente]
    ], { color: "blue", weight: 4, opacity: 0.7 }).addTo(map);

    map.fitBounds(ruta.getBounds(), { padding: [30, 30] });
};

