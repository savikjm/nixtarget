function GeneratePDF(type){
    var companyid = $('.cur_company_id').val();
    var embs = $('.cur_embs').val();

    if (type == 2) {
        $('.loader_short_pdf').show();
        $('.short_pdf_waiting').show();

        $('.short_pdf_confirm').hide();
        $('#generate_short_pdf').hide();
        $('.short_pdf_error').hide();
        $('#short_pdf_error_message').hide();
    } else if (type == 3) {
        $('.loader_blokada_pdf').show();
        $('.blokada_waiting').show();

        $('.blokada_confirm').hide();
        $('#generate_blokada_pdf').hide();
        $('.blokada_error').hide();
        $('#blokada_error_message').hide();
    }
    $.ajax({
        type: 'POST',
        url: "/DocumentHelper.ashx",
        data : "create_pdf=1&type=" + type + "&companyid=" + companyid + "&embs=" + embs,
        dataType: 'text',
        success: function (res) {
            if (res.indexOf("error") > -1) {
                if (type == 2) {
                    $('.short_pdf_error').text(res);
                    if (res.indexOf("Licence error") > -1) {
                        $('.short_pdf_error').text("Грешка!");
                        $('#short_pdf_error_message').show();
                    }
                    $('.short_pdf_error').show();

                    $('.loader_short_pdf').hide();
                    $('.short_pdf_waiting').hide();
                    $('.short_pdf_confirm').hide();
                    $('#generate_short_pdf').hide();
                } else if (type == 3) {
                    $('.blokada_error').text(res);
                    if (res.indexOf("Licence error") > -1) {
                        $('.blokada_error').text("Грешка!");
                        $('#blokada_error_message').show();
                    }
                    $('.blokada_error').show();

                    $('.loader_blokada_pdf').hide();
                    $('.blokada_waiting').hide();
                    $('.blokada_confirm').hide();
                    $('#generate_blokada_pdf').hide();
                }
            }
            else {
                if (type == 2) {
                    window.location = '/Document.ashx?uid=' + res;
                    $('#short_modal').modal('hide');
                } else if (type == 3) {
                    window.location = '/Document.ashx?uid=' + res;
                    $('#blokada_modal').modal('hide');
                }
            }
        },
        error: function (a, b, c) {
            if (type == 2) {
                $('.short_pdf_error').show();

                $('.loader_short_pdf').hide();
                $('.short_pdf_waiting').hide();
                $('.short_pdf_confirm').hide();
                $('#generate_short_pdf').hide();
            } else if (type == 3) {
                $('.blokada_error').show();

                $('.loader_blokada_pdf').hide();
                $('.blokada_waiting').hide();
                $('.blokada_confirm').hide();
                $('#generate_blokada_pdf').hide();
            }
        }
    });
}

function ResetFieldsBlokada(){
    $('.blokada_error').text("Грешка!");
    $('.blokada_confirm').show();
    $('#generate_blokada_pdf').show();

    $('.blokada_waiting').hide();
    $('.blokada_error').hide();
    $('.loader_blokada_pdf').hide();
    $('#blokada_error_message').hide();
}
function ResetFieldsPDF() {
    $('.short_pdf_error').text("Грешка!");
    $('.short_pdf_confirm').show();
    $('#generate_short_pdf').show();

    $('.short_pdf_waiting').hide();
    $('.short_pdf_error').hide();
    $('.loader_short_pdf').hide();
    $('#short_pdf_error_message').hide();
}

function ResetFieldsBonitet(){
    $('.confirm_msg_bonitet').show();
    $('#info_bonitet').show();
    $('#povtrdi_bonitet').show();

    $('.success_msg_bonitet').hide();
    $('.error_msg_bonitet').hide();
    $('.waiting_msg_bonitet').hide();
    $('.loader_bonitet').hide();
    $('#success_bontitet').hide();

}
function ResetFieldsShort() {
    $('.waiting_msg_short').show();
    $('.loader_short').show();

    $('.success_msg_short').hide();
    $('.error_msg_short').hide();
    $('#success_short_msg').hide();
}

function SendEmail(type) {
    var companyid = $('.cur_company_id').val();
    var embs = $('.cur_embs').val();

    if (type == 1)
    {
        $('.waiting_msg_bonitet').show();
        $('.loader_bonitet').show();

        $('.confirm_msg_bonitet').hide();
        $('.success_msg_bonitet').hide();
        $('.error_msg_bonitet').hide();
        $('#info_bonitet').hide();
        $('#povtrdi_bonitet').hide();
        $('#success_bontitet').hide();
    }
    else if (type == 2)
    {
        $('.waiting_msg_short').show();
        $('.loader_short').show();

        $('.success_msg_short').hide();
        $('.error_msg_short').hide();
        $('#success_short_msg').hide();
    }

    $.ajax({
        type: 'POST',
        url: "/DocumentHelper.ashx",
        data: "send_mail=1&companyid=" + companyid + "&embs=" + embs + "&mail_type=" + type,
        dataType: 'text',
        success: function (res) {
            if (res == "Mail Sent") {
                if (type == 1)
                {
                    $('.success_msg_bonitet').show();
                    $('#success_bontitet').show();

                    $('.waiting_msg_bonitet').hide();
                    $('.loader_bonitet').hide();
                    $('.confirm_msg_bonitet').hide();
                    $('.error_msg_bonitet').hide();
                    $('#info_bonitet').hide();
                    $('#povtrdi_bonitet').hide();
                }
                else if (type == 2)
                {
                    $('.success_msg_short').show();
                    $('#success_short_msg').show();

                    $('.waiting_msg_short').hide();
                    $('.loader_short').hide();
                    $('.error_msg_short').hide();
                }
            }
            else {
                if (type == 1)
                {
                    $('.error_msg_bonitet').show();

                    $('#success_bontitet').hide();
                    $('.waiting_msg_bonitet').hide();
                    $('.loader_bonitet').hide();
                    $('.confirm_msg_bonitet').hide();
                    $('#info_bonitet').hide();
                    $('.success_msg_bonitet').hide();
                    $('#povtrdi_bonitet').hide();
                }
                else if (type == 2) {
                    $('.error_msg_short').show();

                    $('.success_msg_short').hide();
                    $('#success_short_msg').hide();
                    $('.waiting_msg_short').hide();
                    $('.loader_short').hide();
                }
            }
        },
        error: function (a, b, c) {
            if (type == 1) {
                $('.error_msg_bonitet').show();

                $('#success_bontitet').hide();
                $('.waiting_msg_bonitet').hide();
                $('.loader_bonitet').hide();
                $('.confirm_msg_bonitet').hide();
                $('#info_bonitet').hide();
                $('.success_msg_bonitet').hide();
                $('#povtrdi_bonitet').hide();
            }
            else if (type == 2) {
                $('.error_msg_short').show();

                $('.success_msg_short').hide();
                $('#success_short_msg').hide();
                $('.waiting_msg_short').hide();
                $('.loader_short').hide();
            }
        }
    });
}

function ResetLostPasswordFields()
{
    $('#msg_txt').show();
    $('.loader_password').hide();
    $('#user_email').val('');
    $('#msg_success').hide();
    $('.modal-title').text("Заборавена лозинка");
    $('#user_email').show();
    $('#email_submit_btn').show();
}
function LostPassword() {
    var email = $('#user_email').val();
    $('.loader_password').show();
    $('#msg_txt').hide();
    $('#user_email').hide();
    $('#email_submit_btn').hide();
    $.ajax({
        type: 'POST',
        url: "/LostPasswordHelper.ashx",
        data: "lost_password=1&email=" + email,
        dataType: 'text',
        success: function (res) {
            $('.loader_password').hide();
            if (res == "Mail Sent") {
                $('#msg_success').show();
                $('.modal-title').text("Успешно праќање!");
            }
            else {
                $('.modal-title').text("Грешка!");
            }
        },
        error: function (a, b, c) {
            $('.loader_password').hide();
            $('#msg_txt').hide();
            $('.modal-title').text("Грешка!");
        }
    });
}

function ResetResetPasswordFields()
{
    $('#reset_msg').show();
    $('#reset_success_msg').hide();
    $('#reset_error_msg').hide();
    $('#reset_expired_msg').hide();
}

function ResetPassword() {
    var email = $('#user_email').val();

    var userid = getParameterByName("userid");
    var activation_code = getParameterByName("activation_code");

    $.ajax({
        type: 'POST',
        url: "/LostPasswordHelper.ashx",
        data: "reset_password=1&userid=" + userid + "&activation_code=" + activation_code,
        dataType: 'text',
        success: function (res) {
            $('#reset_msg').hide();
            if (res == "reset_ok") {
                $('#reset_success_msg').show();
            }
            else if (res == "reset_expired")
            {
                $('#reset_expired_msg').show();
            }
            else{
                $('#reset_error_msg').show();
            }
        },
        error: function (a, b, c) {
            $('#reset_msg').hide();
            $('#reset_error_msg').show();
        }
    });
}

var PackID = 0;
var UserID = 0;
function SetCurPackID(packid)
{
    PackID = packid;
}
function SetCurUserID(userid) {
    UserID = userid;
}
function DisablePack()
{
    if (PackID > 0) {
        $.ajax({
            type: 'POST',
            url: "/Admin/AdminHelper.ashx",
            data: "pack=1&stop_pack=1&packid=" + PackID,
            dataType: 'text',
            success: function (res) {
                if (res == "1")
                    window.location = window.location;
                else
                    alert("Грешка!");
            },
            error: function (a, b, c) {
                alert("Грешка!");
            }
        });
    }
}
function EnablePack() {
    if (PackID > 0) {
        $.ajax({
            type: 'POST',
            url: "/Admin/AdminHelper.ashx",
            data: "pack=1&start_pack=1&packid=" + PackID,
            dataType: 'text',
            success: function (res) {
                if (res == "1")
                    window.location = window.location;
                else
                    alert("Грешка!");
            },
            error: function (a, b, c) {
                alert("Грешка!");
            }
        });
    }
}
function DisableUser() {
    if (UserID > 0) {
        $.ajax({
            type: 'POST',
            url: "/Admin/AdminHelper.ashx",
            data: "user=1&disable_user=1&userid=" + UserID,
            dataType: 'text',
            success: function (res) {
                if (res == "1")
                    window.location = window.location;
                else
                    alert("Грешка!");
            },
            error: function (a, b, c) {
                alert("Грешка!");
            }
        });
    }
}
function EnableUser() {
    if (UserID > 0) {
        $.ajax({
            type: 'POST',
            url: "/Admin/AdminHelper.ashx",
            data: "user=1&enable_user=1&userid=" + UserID,
            dataType: 'text',
            success: function (res) {
                if (res == "1")
                    window.location = window.location;
                else
                    alert("Грешка!");
            },
            error: function (a, b, c) {
                alert("Грешка!");
            }
        });
    }
}
var request_embs;
var request_userid;
function FillDataForRequestMail(embs, userid) {
    request_embs = embs;
    request_userid = userid;
}
function SendRequestMail() {
    $.ajax({
        type: 'POST',
        url: "/DocumentHelper.ashx",
        data: "send_request_mail=1&&embs=" + request_embs + "&userid=" + request_userid,
        dataType: 'text',
        success: function (res) {
            if (res == "Mail Sent")
                window.location = window.location;
            else
                alert("Грешка!");
        },
        error: function (a, b, c) {
            alert("Грешка!");
        }
    });
}

$('document').ready(function () {
    if ($('#start_date_container').length > 0 && $('#end_date_container').length > 0) {
        $('#start_date_container').datetimepicker({
            format: "YYYY-MM-DD",
        });
        $('#end_date_container').datetimepicker({
            format: "YYYY-MM-DD",
        });
    }
});
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}