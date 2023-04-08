Namespace('custom.ui').placePicker = new function () {

    var _defaultPlace = {
        position: {
            lat: 35.69299463209881,
            lng: 51.387451104819775
        }
    };
    var _mapInitialized = false;
    var _afterInitHandler;
    var _map;
    var _geocoder;
    var _marker;
    var _selectedPlace;

    var _ensureMapInited = function (handler) {

        if (!_mapInitialized) {
            $('body').append('<script data-gmaps async defer src="https://maps.googleapis.com/maps/api/js?sensor=false&libraries=places&key=AIzaSyBT7sC0O_yIKwYPoTV7NVHgNAzCxvJCbpw&callback=custom.ui.placePicker._initMap" />');
            _afterInitHandler = handler;
        }
        else {
            handler();
        }
    };
    var _showPlacePicker = function (place, resultHandler) {
        _ensureMapInited(function () {

            if (typeof arguments[0] === 'function') {
                resultHandler = arguments[0];
                place = _defaultPlace;
            }

            if (typeof place === 'undefined' || place === null) {
                place = _defaultPlace
            }

            if (typeof place.position === 'undefined')
                place.position = {};

            if (typeof place.position.lat === 'undefined')
                place.position.lat = _defaultPlace.position.lat;

            if (typeof place.position.lng === 'undefined')
                place.position.lng = _defaultPlace.position.lng;

            var point = new google.maps.LatLng(place.position.lat, place.position.lng);

            _marker.setPosition(point);


            //debugger;
            //_markerPositionChanged();
            $('#myModal .modal-dialog #map-canvas').css('height', ($(window).innerHeight() - ($(window).innerHeight() * 0.3)) + 'px');
            $('#myModal .modal-dialog').css('width', '80%');

            $('#myModal').modal('show');
            $('#myModal').on('shown.bs.modal', function () {
                setTimeout(function () {
                    google.maps.event.trigger(_map, 'resize');
                    _map.setCenter(point);
                    $('#loadholder').remove();
                }, 2000);
            });
            $("#gmapsbutton-ok").click(function () {
                resultHandler(_selectedPlace);
                $('#myModal').modal('hide');
            });
        });
    };

    var __initMap = function () {
        var dialogHtml =
            '<div class="modal fade" id="myModal">' +
            '    <div class="modal-header">' +
            '        <button class="close" data-dismiss="modal">×</button>' +
            '         <h3>Modal header</h3>' +

            '    </div>' +
            '    <div class="modal-body">' +
            '		<div>' +
            '			<input id="pac-input" class="controls" type="text" placeholder="Search Box">' +
            '			<div id="map-canvas"></div>' +
            '		</div>' +
            '	</div>' +
            '</div>';

        dialogHtml =
            '<div id="myModal" class="modal fade">' +
            '    <div class="modal-dialog">' +
            '        <div class="modal-content">' +
            '            <div class="modal-header">' +
            '                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
            '                <h4 class="modal-title">Select a location by moving marker ...</h4>' +
            '            </div>' +
            '            <div class="modal-body">' +
            '               <div id="loadholder" style="position: absolute;width:30%;right: 35%; top: 35%; z-index: 2"><img src="/Content/Images/load.gif" style="width:100%;height:100%"></div>' +
            '				<input id="pac-input" class="controls" type="text" placeholder="Search Box">' +
            '				<div id="map-canvas"></div>' +

            '            </div>' +
            '            <div class="modal-footer">' +
            '                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
            '                <button type="button" class="btn btn-primary" id="gmapsbutton-ok">OK</button>' +
            '            </div>' +
            '        </div>' +
            '    </div>' +
            '</div>';

        var dialogContent = $(dialogHtml);
        $('body').append(dialogContent);

        var centerPoint = new google.maps.LatLng(0, 0);
        var selectedPoint = new google.maps.LatLng(0, 0);

        // Init map
        var options = {
            zoom: 7,
            center: centerPoint,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        _map = new google.maps.Map(document.getElementById('map-canvas'), options);
        _geocoder = new google.maps.Geocoder();

        // Create Marker
        _marker = new google.maps.Marker({ position: selectedPoint, map: _map, draggable: true });
        _marker.setMap(_map);
        google.maps.event.addListener(_marker, 'dragend', _markerPositionChanged);

        google.maps.event.addListener(_map, 'click', function (event) {
            _marker.setPosition(event.latLng);

            _markerPositionChanged();
        });

        // Create the search box and link it to the UI element.
        var input = document.getElementById('pac-input');
        var searchBox = new google.maps.places.SearchBox(input);
        _map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        // Bias the SearchBox results towards current map's viewport.
        _map.addListener('bounds_changed', function () {
            searchBox.setBounds(_map.getBounds());
        });

        // [START region_getplaces]
        searchBox.addListener('places_changed', function () {
            debugger;
            var places = searchBox.getPlaces();

            if (places.length === 0) {
                return;
            }


            var bounds = new google.maps.LatLngBounds();

            _marker.setPosition(places[0].geometry.location);
            _map.panTo(places[0].geometry.location);

            var place = places[0];

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            }
            else {
                bounds.extend(place.geometry.location);
            }

            _map.fitBounds(bounds);

            _markerPositionChanged();
        });
        // [END region_getplaces]

        _mapInitialized = true;

        _afterInitHandler();
    };

    var _markerPositionChanged = function (event) {
        var place = {
            position: {
                lat: _marker.getPosition().lat(),
                lng: _marker.getPosition().lng(),
                title: '',
                address: ''

            }

        };

        _geocoder.geocode({ 'location': _marker.getPosition() }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                    place.address = results[1].formatted_address;
                    _selectedPlace = place;
                }
            }
        });
    }

    this._initMap = __initMap;
    this.showPlacePicker = _showPlacePicker;
};
