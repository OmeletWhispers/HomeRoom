// tableOptions - options for the jQuery DataTable 
var tableOptions = {
    defaults: function () {
        $.extend($.fn.dataTable.defaults, {
            "serverSide": true,
            "ordering": true,
            "searching": true,
            "dom": "rfts",
            "scroller": {
                "loadingIndicator": true
            },
            "scrollY": "400px"
        });
    }
};

(function ($) {
    
    if (!$) {
        return;
    }
   
    abp.event.on('abp.notifications.received', function (userNotification) {
        console.log(userNotification);
    });

    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();
        console.log(data);

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });
        console.log(data);
        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });
        console.log(obj);
        return obj;
    }
})(jQuery);