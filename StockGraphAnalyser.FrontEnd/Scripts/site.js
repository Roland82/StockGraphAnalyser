$(document).ready(function() {

    $(function() {
        var seriesOptions = [],
            seriesCounter = 0,
            dataPoints = [
                { name: 'MovingAverageTwoHundredDay', color: '#FF0000', lineStyle: '' },
                { name: 'MovingAverageFiftyDay', color: '#0047B2', lineStyle: '' },
                { name: 'UpperBollingerBand', color: '#FFAE42', lineStyle: 'longdash' },
                { name: 'LowerBollingerBand', color: '#FFAE42', lineStyle: 'longdash' }
            ];


        $.each(dataPoints, function(i, datapoint) {

            $.getJSON('http://localhost:53110/api/ChartResource/GetInidcator/?symbol=' + $('#symbol').attr('value') + "&indicatorName=" + datapoint.name, function(data) {

                seriesOptions[i] = {
                    color: datapoint.color,
                    name: datapoint.name,
                    data: data.Data,
                    lineWidth: 1,
                    dashStyle: datapoint.lineStyle
                };

                // As we're loading the data asynchronously, we don't know what order it will arrive. So
                // we keep a counter and create the chart when all the data is loaded.
                seriesCounter++;

                if (seriesCounter == dataPoints.length) {


                    $.getJSON('http://localhost:53110/api/chartresource/GetCandleSticks/?ticker=' + $('#symbol').attr('value'), function(candlestickData) {
                        seriesOptions.push({
                            type: 'candlestick',
                            name: $('#symbol').attr('value') + ' Price (p)',
                            data: candlestickData.Data
                        });

                        seriesCounter += 1;
                        createChart();
                    });
                }
            });
        });


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
                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
                    valueDecimals: 2
                },

                series: seriesOptions,
                
                plotOptions: {
                    candlestick: {
                        color: 'red',
                        upColor: 'green'
                    }
                }
            });
        }
    });
});