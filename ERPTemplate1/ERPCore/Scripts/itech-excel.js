$(document).ready(function () {
    $(function () {
        $('input').keyup(function (e) {
            //alert('tdID=' + e.target.id);
            //alert('keyNo=' + e.which);
            var current_Id = e.target.id.split("_");
            var prefix = current_Id[0];
            var lastNum = current_Id[1];
            var nextRowCol = prefix + "_" + (parseInt(lastNum) + 1);
            var prevRowCol = prefix + "_" + (parseInt(lastNum) - 1);

            if (e.which == 39) {
                $(this).closest('td').next().find('input').focus();
            }
            else if (e.which == 37) {
                $(this).closest('td').prev().find('input').focus();
            }
            else if (e.which == 40) {
                //$(this).closest('tr').next().find('td:eq(' + $(this).closest('td').index() + ')').find('input').focus();

                document.getElementById(nextRowCol).focus();

            }
            else if (e.which == 38) {
                //$(this).closest('tr').prev().find('td:eq(' + $(this).closest('td').index() + ')').find('input').focus();
                document.getElementById(prevRowCol).focus();
            }

        });

        $("#ExcelForm :input").change(function (evt) {
            //alert('onchange');
            clkBtn = evt.target.id;
            //alert('clkBtn0=' + clkBtn);
            var current_Id = evt.target.id.split("_");
            var prefix = current_Id[0];
            var lastNum = current_Id[1];

            if (prefix != "txtDebitNoteCode")
            {
                var debitNoteLineCode = $('#txtDebitNoteLineCode_' + lastNum).val();
                var description = $('#txtDescription_' + lastNum).val();
                var qty = parseInt($('#txtQuantity_' + lastNum).val());
                var unitPrice = parseInt($('#txtUnitPrice_' + lastNum).val());
                //alert('debitNoteLineCode=' + debitNoteLineCode);
                //alert('description=' + description);
                //alert('qty=' + qty);
                //alert('unitPrice=' + unitPrice);
                if (debitNoteLineCode != "" && description != "" && qty > 0 && unitPrice > 0)
                {
                    $("#ExcelForm").submit();
                }
            }
            //$("#ExcelForm").submit();
        });

        var clkBtn = "";
        $('input[type="submit"]').click(function (evt) {
            clkBtn = evt.target.id;
            //alert('clkBtn1=' + clkBtn);
        });

        $('#ExcelForm').submit(function (button) {
            var btnID = clkBtn;
            //alert('clkBtn2=' + clkBtn);
            //var lastSubmitChar = btnID[btnID.length - 1];
            //alert("lastSubmitChar" + lastSubmitChar);
            //alert("check=" + $(document.activeElement).val());

            var checkDelete = btnID.indexOf("Delete");
            if (checkDelete >= 0)
            {
                if (!confirm("Are you sure you want to delete ?"))
                {
                    return false;
                }
            }
            document.getElementById("message").innerHTML = "Please wait...";
            $.ajax({
                url: this.action,
                type: this.method,
                cache: false,
                data: $(this).serialize() + "&btnID=" + btnID,
                success: function (data) {
                    document.getElementById("message").innerHTML = "Request is completed.";
                    $('#excel-content').html(data);

                    if (save != "" || cancel != "") {
                        //alert("closing...");
                        //$("#dialog-assign").dialog('close');
                    }
                    
                }
            });
            return false;
        });

        $(".numCol").change(function (evt) {

            //current_Id = evt.target.id;
            //var lastChar = current_Id[current_Id.length - 1];
            var current_Id = evt.target.id.split("_");
            var prefix = current_Id[0];
            var lastNum = current_Id[1];
            var qty = $("#txtQuantity_" + lastNum).val();
            var unitPrice = $("#txtUnitPrice_" + lastNum).val();
            $("#txtTotal_" + lastNum).val(function () {
                var myVar = qty * unitPrice;
                return myVar;
            });
            //$("#txtB").attr("disabled", "").val("Peter");
        });

    });
});