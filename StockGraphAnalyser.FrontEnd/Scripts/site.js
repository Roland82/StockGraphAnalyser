$(document).ready(function() {

    $(function() {
        var seriesOptions = [],
            seriesCounter = 0,
            dataPoints = ['MovingAverageTwoHundredDay', 'MovingAverageFiftyDay'];


        $.each(dataPoints, function(i, name) {

            $.getJSON('http://localhost:53110/api/ChartResource/GetInidcator/?symbol=' + $('#symbol').attr('value') + "&indicatorName=" + name, function(data) {

                seriesOptions[i] = {
                    name: name,
                    data: data.Data
                };

                // As we're loading the data asynchronously, we don't know what order it will arrive. So
                // we keep a counter and create the chart when all the data is loaded.
                seriesCounter++;

                if (seriesCounter == dataPoints.length) {
                    createChart();
                }
            });
        });


        $.getJSON('http://localhost:53110/api/chartresource/GetCandleSticks/?ticker=' + $('#symbol').attr('value'), function(candlestickData) {

            $('#container').highcharts('StockChart', {
                rangeSelector: {
                    inputEnabled: $('#container').width() > 480,
                    selected: 1
                },

                title: {
                    text: $('#symbol').attr('value')
                },

                series: [{
                    type: 'candlestick',
                    name: $('#symbol').attr('value') + ' Price (p)',
                    data: candlestickData.Data
                }],

                plotOptions: {
                    candlestick: {
                        color: 'red',
                        upColor: 'green'
                    }
                },
            });
        });

        // create the chart when all data is loaded

        function createChart() {
            $('#container').highcharts('StockChart', {
                rangeSelector: {
                    inputEnabled: $('#container').width() > 480,
                    selected: 4
                },

                yAxis: {
                    labels: {
                        formatter: function() {
                            return this.value + 'p';
                        }
                    },
                    plotLines: [{
                        value: 0,
                        width: 2,
                        color: 'silver'
                    }]
                },

                tooltip: {
                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
                    valueDecimals: 2
                },

                series: seriesOptions
            });
        }
    });
});