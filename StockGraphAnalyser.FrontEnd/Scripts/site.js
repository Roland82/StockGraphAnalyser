$(document).ready(function () {

    $.getJSON('http://localhost:53110/api/ChartResource/GetProduct/?symbol=' + $('#symbol').attr('value'), function (jsonData) {
        // Create the chart
        $('#container').highcharts('StockChart', {


            rangeSelector: {
                selected: 1,
                inputEnabled: $('#container').width() > 480
            },

            title: {
                text: $('#symbol').attr('value') + ' Stock Price'
            },
            
            xAxis: {
                type: 'datetime'
            },

            series: [{
                name: $('#symbol').attr('value'),
                data: jsonData.Data,
                tooltip: {
                    valueDecimals: 2
                }
            }]
            
            
        }

        );
    });

});
