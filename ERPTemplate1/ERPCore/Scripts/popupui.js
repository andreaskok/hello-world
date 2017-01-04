
    $(document).ready(function () {
        //$("#dialog").dialog();
        //$("#tabs").tabs();
        //alert("ready!");
        var url = "";
        $("#dialog-alert").dialog({
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

        $("#dialog-edit").dialog({
            title: 'Create Record',
            autoOpen: false,
            resizable: false,
            height: 600,
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

        $("#dialog-confirm").dialog({
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

        $("#dialog-detail").dialog({
            title: 'View User',
            autoOpen: false,
            resizable: false,
            height: 600,
            width: 1100,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                //alert("dialog detail");
                $(".ui-dialog-titlebar-close").show();
                $(this).load(url);
                //alert("after loaded");
            }
        });

        $("#lnkCreate").live("click", function (e) {
            //e.preventDefault(); //use this or return false
            url = $(this).attr('href');
            $(".ui-dialog-title").html("Create Record");
            $("#dialog-edit").dialog('open');

            return false;
        });

        $(".lnkEdit").live("click", function (e) {
            // e.preventDefault(); use this or return false
            url = $(this).attr('href');
            //alert(" edit url=" + url);
            $(".ui-dialog-title").html("Update Record");
            $("#dialog-edit").dialog('open');
            //alert("after url =" + url);
            return false;
        });

        $(".lnkDelete").live("click", function (e) {
            // e.preventDefault(); use this or return false
            url = $(this).attr('href');
            $("#dialog-confirm").dialog('open');

            return false;
        });

        $(".lnkDetail").live("click", function (e) {
            // e.preventDefault(); use this or return false
            //alert("detail!");
            url = $(this).attr('href');
            //alert("url=" + url);
            $("#dialog-detail").dialog('open');

            return false;
        });

        $("#btncancel").live("click", function (e) {
            //alert("cancelling");
            $("#dialog-edit").dialog("close");
            //$(this).dialog("close");
            //alert("after cancel");
            return false;
        });

        $("#dialog-workflowsubmit").dialog({
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

        $("#dialog-workflowsubmit").on("dialogclose", function (event, ui) {
            //alert('closing2...');
            location.reload();
        });


        $(".lnkWorkflowSubmit").live("click", function (e) {
            // e.preventDefault(); use this or return false
            url = $(this).attr('href');
            //alert(" edit url=" + url);
            $(".ui-dialog-title").html("Workflow: Submit");
            $("#dialog-workflowsubmit").dialog('open');
            //alert("after url =" + url);
            return false;
        });

        $("#dialog-workflowrecommend").dialog({
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

        $("#dialog-workflowrecommend").on("dialogclose", function (event, ui) {
            //alert('closing2...');
            location.reload();
        });

        $(".lnkWorkflowRecommend").live("click", function (e) {
            // e.preventDefault(); use this or return false
            url = $(this).attr('href');
            //alert(" edit url=" + url);
            $(".ui-dialog-title").html("Workflow: Recommend");
            $("#dialog-workflowrecommend").dialog('open');
            //alert("after url =" + url);
            return false;
        });

        $("#dialog-workflowapprove").dialog({
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

        $("#dialog-workflowapprove").on("dialogclose", function (event, ui) {
            //alert('closing2...');
            location.reload();
        });

        $(".lnkWorkflowApprove").live("click", function (e) {
            // e.preventDefault(); use this or return false
            url = $(this).attr('href');
            //alert(" edit url=" + url);
            $(".ui-dialog-title").html("Workflow: Approve");
            $("#dialog-workflowapprove").dialog('open');
            //alert("after url =" + url);
            return false;
        });
    });

