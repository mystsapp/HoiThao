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

    deleteUser: function (id) {
        $.ajax({
            url: '/account/Delete',
            data: {
                id: id
            },
            dataType: 'json',
            type: 'POST',
            success: function (response) {
                if (response.status) {
                    bootbox.alert({
                        size: "small",
                        title: "Delete Infomation",
                        message: "Đã xóa thành cong.",
                        callback: function () {
                            layoutController.LoadData(true);
                        }
                    });
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Delete Infomation",
                        message: response.message
                    });
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
};
layoutController.init();