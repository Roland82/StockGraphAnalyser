$(function () {

    $.getJSON('http://localhost:53110/api/ChartResource/GetProduct', function (data) {
        // Create the chart
        $('#container').highcharts('StockChart', {


            rangeSelector: {
                selected: 1,
                inputEnabled: $('#container').width() > 480
            },

            title: {
                text: 'AAPL Stock Price'
            },

            series: [{
                name: 'AAPL',
                data: data,
                tooltip: {
                    valueDecimals: 2
                }
            }]
        });
    });

});
