var layoutController = {
    init: function () {
        layoutController.registerEvent();

    },

    registerEvent: function () {

        $('#ImportData').off('click').on('click', function (event) {
            event.preventDefault();
            $('#modalExcelModel').modal('show');

            $('#btnImport').off('click').on('click', function () {
                var data = new FormData();
                var files = $("#FileUpload").get(0).files;

                if (files.length > 0) { data.append("HelpSectionImages", files[0]); }
                else {
                    //common.showNotification('warning', 'Please select file to upload.', 'top', 'right');
                    bootbox.alert({
                        size: "small",
                        title: "warning",
                        message: 'Please select excel file to upload.'
                    });
                    return false;
                }
                console.log(data);
                layoutController.importData(data);
            });
        });

        $('#HotelList').off('click').on('click', function (event) {
            event.preventDefault();
            $('#hotelModal').modal('show');

            layoutController.loadDdlHotel();

            $('#btnReport').off('click').on('click', function () {
                //            $('#hotelModal').modal('show');
                var hotel = $('#ddlHotel').val();
                //alert(hotel);
                //layoutController.ExportHotel(hotel);
                $('#hidHotel').val(hotel);
                $('#frmHotel').on('submit');
                $('#hotelModal').modal('hide');
            });
        });

        
    },

    importData: function (data) {
        $.ajax({
            url: '/Home/UploadExcel',
            type: 'POST',
            processData: false,
            data: data,
            dataType: 'json',
            contentType: false,
            success: function (response) {
                if (response.status) {
                    //console.log(response.message);
                    bootbox.alert({
                        size: "small",
                        title: "Upload Infomation",
                        message: response.message,
                        callback: function () {
                            //layoutController.LoadData(true);
                            $('#modalExcelModel').modal('hide');
                        }
                    });
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Upload Infomation",
                        message: response.message
                    });
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    loadDdlHotel: function () {
        $('#ddlHotel').html('');
        var option = '';
        // option = option + '<option value=select>Select</option>';

        $.ajax({
            url: '/Home/GetAllHotel',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                //console.log(response.data);
                
                //if (response.length > 0) {
                //var data = JSON.stringify(response.data);
                var data = JSON.parse(response.data);


                for (var i = 0; i < data.length; i++) {
                    // set the key/property (input element) for your object
                    var ele = data[i];
                    if (ele === null)
                        ele = 'Other';
                    option = option + '<option value="' + ele + '">' + ele + '</option>'; //chinhanh1
                    // add the property to the object and set the value
                    //params[ele] = $('#' + ele).val();
                }
                $('#ddlHotel').html(option);

            }
        });
        //homeController.resetForm();
    },

    //ExportHotel: function (hotel) {

    //    $.ajax({
    //        url: '/Report/ExportHotel',
    //        type: 'POST',
    //        data: {
    //            hotel: hotel
    //        },
    //        dataType: 'json',
    //        success: function (response) {
    //            if (response.status) {
    //                bootbox.alert({
    //                    size: "small",
    //                    title: "Export Infomation",
    //                    message: response.message,
    //                    callback: function () {
    //                        //layoutController.LoadData(true);
    //                        $('#hotelModal').modal('hide');
    //                    }
    //                });
    //            }
    //        }
    //    });
    //    //homeController.resetForm();
    //}
};
layoutController.init();