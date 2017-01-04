
$(document).ready(function () {
    //$("#dialog").dialog();
    //alert("ready!");
    var url = "";
    

    $("#dialog-assign").dialog({
        title: 'List Box',
        autoOpen: false,
        resizable: false,
        height: 500,
        width: 700,
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



    $(".lnkAssign").live("click", function (e) {
        // e.preventDefault(); use this or return false
        url = $(this).attr('href');
        //alert(" edit url=" + url);
        $(".ui-dialog-title").html("Assign Value");
        $("#dialog-assign").dialog('open');
        //alert("after url =" + url);
        return false;
    });

  


   


});

