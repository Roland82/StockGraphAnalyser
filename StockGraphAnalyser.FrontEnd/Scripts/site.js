$(function () {

    $.getJSON('http://localhost:53110/api/ChartResource/GetProduct', function (jsonData) {
        // Create the chart
        $('#container').highcharts('StockChart', {


            rangeSelector: {
                selected: 1,
                inputEnabled: $('#container').width() > 480
            },

            title: {
                text: 'AAPL Stock Price'
            },
            
            xAxis: {
                type: 'datetime'
            },

            series: [{
                name: 'AAPL',
                data: jsonData.Data,
                tooltip: {
                    valueDecimals: 2
                }
            }]
            
            
        }

        );
    });

});
