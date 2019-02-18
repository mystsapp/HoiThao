var homeconfig = {
    pageSize: 5,
    pageIndex: 1
};
//returns a Date() object in dd/MM/yyyy
$.formattedDate = function (dateToFormat) {
    var dateObject = new Date(dateToFormat);

    var day = dateObject.getDate();
    var month = dateObject.getMonth() + 1;
    var year = dateObject.getFullYear();
    day = day < 10 ? "0" + day : day;
    month = month < 10 ? "0" + month : month;
    var formattedDate = day + "/" + month + "/" + year;
    return formattedDate;
};

$.formattedDateTime = function (dateToFormat) {
    var dateObject = new Date(dateToFormat);

    if (dateObject.getHours() >= 12) {
        var hour = parseInt(dateObject.getHours()) - 12;
        var amPm = "PM";
    } else {
        var hour = dateObject.getHours();
        var amPm = "AM";
    }
    var time = hour + ":" + dateObject.getMinutes() + ":" + dateObject.getSeconds() + " " + amPm;

    var day = dateObject.getDate();
    var month = dateObject.getMonth() + 1;
    var year = dateObject.getFullYear();
    day = day < 10 ? "0" + day : day;
    month = month < 10 ? "0" + month : month;
    var formattedDate = day + "/" + month + "/" + year + " " + time;
    return formattedDate;
};


$.stringToDate = function (_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
};


$.getMyFormatDate = function (date) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var d = date;
    var hours = d.getHours();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    return months[d.getMonth()] + ' ' + d.getDate() + " " + d.getFullYear() + ' ' + hours + ':' + d.getMinutes() + ' ' + ampm;
}

//var dateObj = new Date();
//var dateFormat = getMyFormatDate(dateObj); // Jun 29 2016 12:40 AM


$.timeFormatter = function (dateTime) {
    var date = new Date(dateTime);
    if (date.getHours() >= 12) {
        var hour = parseInt(date.getHours()) - 12;
        var amPm = "PM";
    } else {
        var hour = date.getHours();
        var amPm = "AM";
    }
    var time = hour + ":" + date.getMinutes() + " " + amPm;
    console.log(time);
    return time;
};

var homeController = {
    init: function () {
        homeController.LoadData();
    },

    registerEvent: function () {

        //$('.modal-dialog').draggable();

        $(".payment").each(function () {
            var payment = $(this).text();
            if (payment === 'PAID') {
                $('.firstname').css("background-color", "green").css("color", "white");
            }
            else
                $('.firstname').css("background-color", "red").css("color", "white");
        });

        $('#frmSaveData').validate({
            rules: {
                username: {
                    required: true,
                    minlength: 3
                },
                password: {
                    //required: true,
                    minlength: 3
                },
                hoten: {
                    required: true
                }
            },
            messages: {
                username: {
                    required: "Trường này không được để trống.",
                    minlength: "Username phải có ít nhất 3 ký tự"
                },
                password: {
                    required: "Trường này không được để trống.",
                    //number: "password phải là số",
                    minlength: "Password phải có ít nhất 3 ký tự"
                },
                hoten: {
                    required: "Trường này không được để trống."
                }
            }
        });


        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtId').prop("disabled", true);

            homeController.resetForm();

            homeController.getAseanId();

            //homeController.loadDdlChiNhanh();
            //homeController.loadDdlDaiLy();
        });


        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                homeController.saveData();
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            homeController.LoadData(true);
        });

        $('#txtNameS').off('keypress').on('keypress', function (e) {
            if (e.which === 13)
                homeController.LoadData(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.LoadData(true);
        });

        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');


            var kId = $(this).data('kid');
            // $('#txtHidID').val(id);

            // $('#txtUsername').prop("disabled", true);

            homeController.loadDetail(kId);


            //if ($('#ddlNhom').val() != "Users") {

            //    $('#ddlChiNhanh').prop("disabled", true);
            //    $('#ddlDaiLy').prop("disabled", true);
            //}

        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('kid');
            bootbox.confirm({
                title: "Delete Confirm?",
                message: "Are you sure?",
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-times"></i> Cancel'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> Confirm'
                    }

                },
                callback: function (result) {
                    if (result) {
                        homeController.deleteAsean(id);
                    }
                }

            });
        });

        //$("#txtNgaySinh, #txtNgayCMND, #txtHanTheHDV, #txtHieuLucHoChieu, #txtHanVisa").datepicker({
        //    changeMonth: true,
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy"

        //});
        $('.txtCheckin').off('keydown').on('keydown', function (e) {
            var d = new Date();
            if (e.which === 113) {//F2
                //var dt = new Date();
                //var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                //var datetime = dt.getDate() + '/' + (dt.getMonth() + 1) + '/' + dt.getFullYear() + ' ' + $.timeFormatter(dt.getTime());
                //alert(datetime);
                //$(this).val(d);

                var k = $(this).data('kid');

                //var invited = $('#invited').text();


                homeController.updateFromCheckin(d, k);

                //var d = new Date('2011-08-31T20:01:32.000Z');
                //var date = d.format("dd-mm-yyyy");
                //var time = d.format("HH:MM");
                //alert(date);


                // alert(d);
            }
        });

        $('.aseanTr').off('click').on('click', function () {
            var k = $(this).data('kid');
            homeController.getDetail(k);
        }).hover(function () {
            
            $(this).toggleClass('hoverClass');
        });

        $('.btn-PrintReceipt').off('click').on('click', function () {
            var id = $(this).data('kid');
            window.open('/Home/PrintReceipt?id=' + id);

        });

        $('.btn-PrintVAT').off('click').on('click', function () {
            var id = $(this).data('kid');
            window.open('/Home/PrintVAT?id=' + id);

        });
    },

    getDetail: function (id) {
        $.ajax({
            url: '/Home/GetDetail',
            data: {
                id: id
            },
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    var data = response.data;

                    if (data.checkin === null)
                        ck = "";
                    else {
                        ck = $.formattedDateTime(new Date(parseInt(data.checkin.substr(6))));
                    }

                    if (data.checkin === null)
                        ckHid = "";
                    else {
                        ckHid = $.formattedDate(new Date(parseInt(data.checkin.substr(6))));
                    }

                    if (data.dangky === null)
                        dk = "";
                    else {
                        dk = $.formattedDateTime(new Date(parseInt(data.dangky.substr(6))));
                    }
                    $('#Hotel').text(data.Hotel);
                    $('#HotelCheckin').text(data.HotelCheckin);
                    $('#HotelChceckout').text(data.HotelCheckout);
                    $('#HotelPrice').text(data.HotelPrice);
                    $('#HotelBookingInf').text(data.HotelBookingInf);
                    $('#At').text(data.at);

                    $('#Dep').text(data.dt);

                    $('#Address').text(data.email);
                    $('#Department').text(data.email);
                    $('#Institution').text(data.email);
                    $('#Note').text(data.dfno);

                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Detail User Infomation",
                        message: response.message
                    });
                }
            }
        });
    },

    updateFromCheckin: function (d, k) {

        var checkin = d;

        //var hidId = parseInt(k);
        var asean = {
            k: k,
            checkin: checkin//,
        };


        $.ajax({
            url: '/Home/UpdateCheckin',
            data: {
                strAsean: JSON.stringify(asean)
            },
            dataType: 'json',
            type: 'POST',
            success: function (response) {
                if (response.status) {
                    bootbox.alert({
                        size: "small",
                        title: "Save Infomation",
                        message: response.message,
                        callback: function () {
                            $('#modalAddUpdate').modal('hide');
                            homeController.LoadData(true);
                        }
                    });
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Save Infomation",
                        message: response.message
                    });

                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    getAseanId: function () {
        $.ajax({
            url: '/Home/GetLastId',
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    //console.log(response.data);
                    var data = response.data;
                    homeController.nextAseanId(data);
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Get Lats User ID Infomation",
                        message: response.message
                    });
                }
            }
        });
    },

    nextAseanId: function (data) {
        $.ajax({
            url: '/Home/GetNextId',
            data: {
                id: data
            },
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    var nextId = response.data;
                    $('#txtId').val(nextId);
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Get Next User ID Infomation",
                        message: response.message
                    });
                }
            }
        });
    },

    deleteAsean: function (id) {
        $.ajax({
            url: '/Home/Delete',
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
                        message: "Delete success.",
                        callback: function () {
                            homeController.LoadData(true);
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
    },

    saveData: function () {
        var invited = $('#ckInvited').prop('checked');
        var speaker = $('#ckSpeaker').prop('checked');
        var id = $('#txtId').val();
        var title = $('#txtTitle').val();
        var firstname = $('#txtFirstname').val();
        var lastname = $('#txtLastname').val();
        var company = $('#txtCompany').val();
        var email = $('#txtEmail').val();

        var tel = $('#txtPhone').val();
        var country = $('#txtCountry').val();
        var payment = $('#txtPayment').val();
        var amount = $('#txtAmount').val();
        var bankfee = $('#txtBankfee').val();
        var mop = $('#txtMop').val();

        var cartnumber = $('#txtCartnumber').val();
        var currency = $('#txtCurrency').val();
        var rate = $('#txtRate').val();
        var grandtotal = $('#txtGrandtotal').val();

        //var checkin = $('#txtCheckin').val();

        var descript = $('#txtDescript').val();

        var vatbill = $('#txtVatBill').val();
        var taxcode = $('#txtTaxCode').val();
        var fax = $('#txtFax').val();
        var dangky = $('#txtDangky').val();
        var totala = $('#txtTotala').val();
        var totalb = $('#txtTotalb').val();

        //var ck = checkin;

        var dk = dangky;

        //if (ck === "")
        //    ck = "";
        //else
        //    ck = $.stringToDate(ck, 'dd/MM/yyyy', '/');

        if (dk === "")
            dk = "";
        else
            dk = $.stringToDate(dk, 'dd/MM/yyyy', '/');

        //var ngaycapnhat = $('#txtNgayCapNhat').val();
        //var ncn = $.stringToDate(ngaycapnhat, 'dd/MM/yyyy', '/');

        //var hidPass = $('#txtHidPass').val();

        //var status = $('#ckStatus').prop('checked');
        var hidId = $('#hidID').val();
        var asean = {
            invited: invited,
            speaker: speaker,
            id: id,
            title: title,
            firstname: firstname,

            lastname: lastname,
            company: company,
            email: email,
            tel: tel,
            country: country,

            payment: payment,
            amount: amount,
            bankfee: bankfee,
            mop: mop,
            cartnumber: cartnumber,

            currency: currency,
            rate: rate,
            grandtotal: grandtotal,
            //checkin: ck,
            descript: descript,

            vatbill: vatbill,
            taxcode: taxcode,
            fax: fax,
            dangky: dk,
            totala: totala,
            totalb: totalb

            //ngaytao: nt,
            //ngaycapnhat: ncn

        };
        $.ajax({
            url: '/Home/SaveData',
            data: {
                strAsean: JSON.stringify(asean),
                Hidid: hidId
            },
            dataType: 'json',
            type: 'POST',
            success: function (response) {
                if (response.status) {
                    bootbox.alert({
                        size: "small",
                        title: "Save Infomation",
                        message: response.message,
                        callback: function () {
                            $('#modalAddUpdate').modal('hide');
                            homeController.LoadData(true);
                        }
                    });
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Save Infomation",
                        message: response.message
                    });

                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    loadDetail: function (id) {
        $.ajax({
            url: '/Home/GetDetail',
            data: {
                id: id
            },
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                if (response.status) {
                    var data = response.data;

                    if (data.checkin === null)
                        ck = "";
                    else {
                        ck = $.formattedDateTime(new Date(parseInt(data.checkin.substr(6))));
                    }

                    if (data.checkin === null)
                        ckHid = "";
                    else {
                        ckHid = $.formattedDate(new Date(parseInt(data.checkin.substr(6))));
                    }

                    if (data.dangky === null)
                        dk = "";
                    else {
                        dk = $.formattedDateTime(new Date(parseInt(data.dangky.substr(6))));

                    }

                    $('#hidID').val(data.k);
                    $('#ckInvited').prop('checked', data.invited);
                    $('#ckSpeaker').prop('checked', data.speaker);
                    $('#txtId').val(data.id);

                    //$('#ddlPhai').val(gioitinh);
                    //$('#ddlPhai').prop('selectedIndex', !gioitinh);
                    //$("#ddlPhai").selectedIndex = gioitinh;

                    $('#txtTitle').val(data.title);
                    $('#txtFirstname').val(data.firstname);
                    $('#txtLastname').val(data.lastname);
                    $('#txtCompany').val(data.company);
                    $('#txtEmail').val(data.email);
                    $('#txtPhone').val(data.tel);

                    $('#txtCountry').val(data.country);
                    $('#txtPayment').val(data.payment);
                    $('#txtAmount').val(data.amount);
                    $('#txtBankfee').val(data.bankfee);
                    $('#txtMop').val(data.mop);
                    $('#txtCartnumber').val(data.cardnumber);

                    $('#txtCurrency').val(data.currency);
                    $('#txtRate').val(data.rate);
                    $('#txtGrandtotal').val(data.grandtotal);

                    $('#txtCheckin').val(ck);
                    $('#txtCheckinHid').val(ckHid);

                    $('#txtCheckin').prop('disabled', true);

                    $('#txtDescript').val(data.descript);
                    $('#txtVatBill').val(data.vatbill);

                    $('#txtTaxCode').val(data.taxcode);
                    $('#txtFax').val(data.fax);
                    $('#txtDangky').val(dk);
                    $('#txtDangky').prop('disabled', true);

                    $('#txtTotala').val(data.totala);
                    $('#txtTotalb').val(data.totalb);
                }
                else {
                    bootbox.alert({
                        size: "small",
                        title: "Detail User Infomation",
                        message: response.message
                    });
                }
            }
        });
    },

    resetForm: function () {
        $('#ckInvited').prop('checked', false);
        $('#ckSpeaker').prop('checked', false);
        $('#hidID').val('0');
        $('#txtId').val('');
        $('#txtTitle').val('');
        $('#txtFirstname').val('');
        $('#txtLastname').val('');
        $('#txtCompany').val('');
        $('#txtEmail').val('');
        $('#txtPhone').val('');
        $('#txtCountry').val('');
        $('#txtPayment').val('');

        $('#txtAmount').val('0');
        $('#txtBankfee').val('');
        $('#txtMop').val('');
        $('#txtCartnumber').val('');
        $('#txtCurrency').val('');
        $('#txtRate').val('');
        $('#txtGrandtotal').val('');
        $('#txtCheckin').val('');
        $('#txtDescript').val('');
        $('#txtVatBill').val('');

        $('#txtTaxCode').val('');
        $('#txtFax').val('');
        $('#txtDangky').val('');
        $('#txtTotala').val('');
        $('#txtTotalb').val('');
    },


    LoadData: function (changePageSize) {
        var name = $('#txtNameS').val();
        var status = $('#ddlStatusS').val();

        $.ajax({
            url: '/Home/LoadData',
            type: 'GET',
            data: {
                name: name,
                status: status,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                //console.log(response.data);
                if (response.status) {
                    //console.log(response.data);
                    var data = response.data;
                    //var data = JSON.parse(response.data);

                    //alert(data);
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        //usage:
                        //var formattedDate = $.formattedDate(new Date(parseInt(item.ngaysinh.substr(6))));
                        //alert(formattedDate)


                        var ci = "";
                        if (item.checkin === null)
                            ci = "";
                        else
                            ci = $.formattedDateTime(new Date(parseInt(item.checkin.substr(6))));

                        var dk = "";
                        if (item.dangky === null)
                            dk = "";
                        else
                            dk = $.formattedDateTime(new Date(parseInt(item.dangky.substr(6))));

                        var inv = item.invited;
                        if (inv !== null)
                            inv = item.invited === true ? "<span class=\"label label-success invited2\">true</span>" : "<span class=\"label label-warning invited2\">false</span>";
                        else
                            inv = '';

                        var spk = item.speaker;
                        if (spk !== null)
                            spk = item.speaker === true ? "<span class=\"label label-success speaker1\">true</span>" : "<span class=\"label label-warning speaker1\">false</span>";
                        else
                            spk = '';

                        html += Mustache.render(template, {
                            kId: item.k,
                            invited: inv,
                            speaker: spk,
                            id: item.id,
                            title: item.title,
                            firstname: item.firstname,

                            lastname: item.lastname,
                            company: item.company,
                            email: item.email,
                            tel: item.tel,
                            country: item.country,

                            payment: item.payment,
                            amount: numeral(item.amount).format('0,0'),
                            bankfee: numeral(item.bankfee).format('0,0'),
                            mop: item.mop,
                            cardnumber: item.cardnumber,

                            currency: item.currency,
                            rate: numeral(item.rate).format('0,0'),
                            grandtotal: numeral(item.grandtotal).format('0,0'),
                            checkin: ci,
                            descript: item.descript,

                            vatbill: item.vatbill,
                            taxcode: item.taxcode,
                            fax: item.fax,
                            dangky: dk,
                            totala: numeral(item.totala).format('0,0'),

                            totalb: numeral(item.totalb).format('0,0')

                        });

                    });
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.LoadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        });
    },

    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);//lam tron len

        //unbind pagination if it existed or click change size ==> reset
        if (('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData('twbsPagination');
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "trước",
            visiblePages: 10, // tong so trang hien thi , ...12345678910...
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;//khi chuyen trang thi set lai page index
                setTimeout(callback, 200);
            }
        });
    }
};
homeController.init();