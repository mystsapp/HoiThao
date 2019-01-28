var reportController = {
    init: function () {
        reportController.registerEvent();

    },

    registerEvent: function () {

        //$('#Conference').off('click').on('click', function (e) {
        //    e.preventDefault();

        //    reportController.ConferenceReport();
        //});



    },

    //ConferenceReport: function () {

    //    $.ajax({
    //        url: '/Report/ConferenceReport',
    //        dataType: 'json',
    //        type: 'POST',
    //        success: function (response) {
    //            console.log(response);
    //            if (response.status) {

    //                bootbox.alert({
    //                    size: "small",
    //                    title: "Report Infomation",
    //                    message: "Report success."
    //                });
    //            }
    //            else {
    //                bootbox.alert({
    //                    size: "small",
    //                    title: "Report Infomation",
    //                    message: response.message
    //                });
    //            }
    //        },
    //        error: function (err) {
    //            console.log(err);
    //        }
    //    });
    //},

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
                            reportController.LoadData(true);
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
reportController.init();