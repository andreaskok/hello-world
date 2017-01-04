
$(document).ready(function () {
    //$("#dialog").dialog();
    //alert("ready!");
    var url = "";
    $("#dialog-alert2").dialog({
        autoOpen: false,
        resizable: false,
        height: 170,
        width: 350,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").show();
        },
        buttons: {
            "OK": function () {
                $(this).dialog("close");

            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    if ('@TempData["msg"]' != "") {
        //$("#dialog-alert").dialog('open');
    }

    $("#dialog-edit2").dialog({
        title: 'Create Record',
        autoOpen: false,
        resizable: false,
        height: 500,
        width: 1100,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            //alert("dialog edit"); //20160609: should popup here
            $(".ui-dialog-titlebar-close").show();
            $(this).load(url);
            //alert("after loaded");
        }
    });

    $("#dialog-confirm2").dialog({
        autoOpen: false,
        resizable: false,
        height: 170,
        width: 350,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").show();

        },
        buttons: {
            "OK": function () {
                $(this).dialog("close");
                window.location.href = url;
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialog-detail2").dialog({
        title: 'View User',
        autoOpen: false,
        resizable: false,
        height: 500,
        width: 1100,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            //alert("dialog detail");
            $(".ui-dialog-titlebar-close").show();
            $(this).load(url);
            //alert("after loaded");
        },
        buttons: {
            "Close": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#lnkCreate2").live("click", function (e) {
        //e.preventDefault(); //use this or return false
        url = $(this).attr('href');
        $(".ui-dialog-title").html("Create Record");
        $("#dialog-edit2").dialog('open');

        return false;
    });

    $(".lnkEdit2").live("click", function (e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        //alert(" edit url=" + url);
        $(".ui-dialog-title").html("Update Record");
        $("#dialog-edit2").dialog('open');
        //alert("after url =" + url);
        return false;
    });

    $(".lnkDelete2").live("click", function (e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        $("#dialog-confirm2").dialog('open');

        return false;
    });

    $(".lnkDetail2").live("click", function (e) {
        // e.preventDefault(); use this or return false
        //alert("detail!");
        url = $(this).attr('href');
        //alert("url=" + url);
        $("#dialog-detail2").dialog('open');

        return false;
    });

    $("#btncancel2").live("click", function (e) {
        //alert("cancelling");
        $("#dialog-edit2").dialog("close");
        //$(this).dialog("close");
        //alert("after cancel");
        return false;
    });

    $("#btnsaveclose2").live("click", function (e) {
        //alert("cancelling");
        $("#dialog-edit2").dialog("close");
        //$(this).dialog("close");
        //alert("after cancel");
        return false;
    });

    $("#dialog-workflowsubmit2").dialog({
        title: 'List Box',
        autoOpen: false,
        resizable: false,
        height: 200,
        width: 380,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            //alert("dialog edit"); //20160609: should popup here
            $(".ui-dialog-titlebar-close").show();
            $(this).load(url);
            //alert("after loaded");
        },
        close: function (event, ui) {
            //alert('closing1...');
            location.reload();
        }

    });

    $("#dialog-workflowsubmit2").on("dialogclose", function (event, ui) {
        //alert('closing2...');
        location.reload();
    });


    $(".lnkWorkflowSubmit2").live("click", function (e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        //alert(" edit url=" + url);
        $(".ui-dialog-title").html("Workflow: Submit");
        $("#dialog-workflowsubmit2").dialog('open');
        //alert("after url =" + url);
        return false;
    });

    $("#dialog-workflowrecommend2").dialog({
        title: 'List Box',
        autoOpen: false,
        resizable: false,
        height: 200,
        width: 380,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            //alert("dialog edit"); //20160609: should popup here
            $(".ui-dialog-titlebar-close").show();
            $(this).load(url);
            //alert("after loaded");
        },
        close: function (event, ui) {
            //alert('closing1...');
            location.reload();
        }
    });

    $("#dialog-workflowrecommend2").on("dialogclose", function (event, ui) {
        //alert('closing2...');
        location.reload();
    });

    $(".lnkWorkflowRecommend2").live("click", function (e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        //alert(" edit url=" + url);
        $(".ui-dialog-title").html("Workflow: Recommend");
        $("#dialog-workflowrecommend2").dialog('open');
        //alert("after url =" + url);
        return false;
    });

    $("#dialog-workflowapprove2").dialog({
        title: 'List Box',
        autoOpen: false,
        resizable: false,
        height: 200,
        width: 380,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            //alert("dialog edit"); //20160609: should popup here
            $(".ui-dialog-titlebar-close").show();
            $(this).load(url);
            //alert("after loaded");
        },
        close: function (event, ui) {
            //alert('closing1...');
            location.reload();
        }
    });

    $("#dialog-workflowapprove2").on("dialogclose", function (event, ui) {
        //alert('closing2...');
        location.reload();
    });

    $(".lnkWorkflowApprove2").live("click", function (e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        //alert(" edit url=" + url);
        $(".ui-dialog-title").html("Workflow: Approve");
        $("#dialog-workflowapprove2").dialog('open');
        //alert("after url =" + url);
        return false;
    });

});

