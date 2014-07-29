//$(document).ready(function() {

//    $(function() {
//        var seriesOptions = [],
//            volume,
//            seriesCounter = 0,
//            dataPoints = [
//                { name: 'MovingAverageTwoHundredDay', color: '#FF0000', lineStyle: '' },
//                { name: 'MovingAverageFiftyDay', color: '#0047B2', lineStyle: '' },
//                { name: 'UpperBollingerBand', color: '#FFAE42', lineStyle: 'longdash' },
//                { name: 'LowerBollingerBand', color: '#FFAE42', lineStyle: 'longdash' }
//            ];
        
//        $.getJSON('http://localhost:53110/api/ChartResource/GetInidcator/?symbol=' + $('#symbol').attr('value') + "&indicatorName=Volume", function(data) {
//            volume = {
//                name: "Volume",
//                data: data.Data
//            };
//        });

//        alert(volume);
//        $.each(dataPoints, function(i, datapoint) {

//            $.getJSON('http://localhost:53110/api/ChartResource/GetInidcator/?symbol=' + $('#symbol').attr('value') + "&indicatorName=" + datapoint.name, function(data) {

//                seriesOptions[i] = {
//                    color: datapoint.color,
//                    name: datapoint.name,
//                    data: data.Data,
//                    lineWidth: 1,
//                    dashStyle: datapoint.lineStyle
//                };

//                // As we're loading the data asynchronously, we don't know what order it will arrive. So
//                // we keep a counter and create the chart when all the data is loaded.
//                seriesCounter++;

//                if (seriesCounter == dataPoints.length) {


//                    $.getJSON('http://localhost:53110/api/chartresource/GetCandleSticks/?ticker=' + $('#symbol').attr('value'), function(candlestickData) {
//                        seriesOptions.push({
//                            type: 'candlestick',
//                            name: $('#symbol').attr('value') + ' Price (p)',
//                            data: candlestickData.Data
//                        });

//                        seriesCounter += 1;
//                        createChart();
//                    });
//                }
//            });
//        });






//        function createForceIndexChart() {
//            $('#force-index-container').highcharts('StockChart', {
//                rangeSelector: {
//                    inputEnabled: $('#container').width() > 480,
//                    selected: 4
//                },

//                yAxis: [{
//                    labels: {
//                        formatter: function () {
//                            return this.value;
//                        }
//                    },
             
//                    plotLines: [{
//                        value: 0,
//                        width: 2,
//                        color: 'silver'
//                    }]
//                }, {
                    
//                }],

//                tooltip: {
//                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
//                    valueDecimals: 2
//                },

//                series: seriesOptions,
//            });
//        }





//        function createChart() {
//            $('#container').highcharts('StockChart', {
//                rangeSelector: {
//                    inputEnabled: $('#container').width() > 480,
//                    selected: 4
//                },

//                yAxis: [{
//                    labels: {
//                        formatter: function() {
//                            return this.value + 'p';
//                        }
//                    },
//                    plotLines: [{
//                        value: 0,
//                        width: 2,
//                        color: 'silver'
//                    }]
//                }, {
//                    labels: {
//                        align: 'right',
//                        x: -3
//                    },
//                    title: {
//                        text: 'Volume'
//                    },
//                    top: '65%',
//                    height: '35%',
//                    offset: 0,
//                    lineWidth: 2
//                }],

//                tooltip: {
//                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
//                    valueDecimals: 2
//                },

//                series: seriesOptions,
                
//                plotOptions: {
//                    candlestick: {
//                        color: 'red',
//                        upColor: 'green'
//                    }
//                }
//            });
//        }
//    });
//});

$(function() {
    $.getJSON('http://localhost:53110/api/ChartResource/Get/?symbol=' + $('#symbol').attr('value'), function(data) {

        // split the data set into ohlc and volume
        var ohlc = [],
            volume = [],
            forceIndexOneDay = [],
            forceIndexThirteenDay = [],
            dataLength = data.Data.length;

        for (i = 0; i < dataLength; i++) {
            ohlc.push([
                data.Data[i][0], // the date
                data.Data[i][1], // open
                data.Data[i][2], // high
                data.Data[i][3], // low
                data.Data[i][4] // close
            ]);

            volume.push([
                data.Data[i][0], // the date
                data.Data[i][5] // the volume
            ]);

            forceIndexOneDay.push([
                data.Data[i][0],
                data.Data[i][6]
            ]);

            forceIndexThirteenDay.push([
                data.Data[i][0],
                data.Data[i][7]
            ]);
        }

        // set the allowed units for data grouping
        var groupingUnits = [[
                'week', // unit name
                [1]                             // allowed multiples
            ], [
                'month',
                [1, 2, 3, 4, 6]
            ]];

        // create the chart
        $('#container').highcharts('StockChart', {
            rangeSelector: {
                inputEnabled: $('#container').width() > 480,
                selected: 1
            },

            title: {
                text: 'AAPL Historical'
            },

            yAxis: [{
                    labels: {
                        align: 'right',
                        x: -3
                    },
                    title: {
                        text: 'OHLC'
                    },
                    height: '40%',
                    lineWidth: 2
                },
                {
                    labels: {
                        align: 'right',
                        x: -3
                    },
                    title: {
                        text: 'Volume'
                    },
                    top: '45%',
                    height: '10%',
                    offset: 0,
                    lineWidth: 2
                },
                {
                    labels: {
                        align: 'right',
                        x: -3
                    },
                    title: {
                        text: 'Force 1 Day'
                    },
                    top: '60%',
                    height: '20%',
                    offset: 0,
                    lineWidth: 2
                },

                 {
                     labels: {
                         align: 'right',
                         x: -3
                     },
                     title: {
                         text: 'Force 13 Day'
                     },
                     top: '80%',
                     height: '20%',
                     offset: 0,
                     lineWidth: 2
                 }
            ],

            series: [{
                    type: 'candlestick',
                    name: 'AAPL',
                    data: ohlc
                },
                {
                    type: 'column',
                    name: 'Volume',
                    data: volume,
                    yAxis: 1
                },
                {
                    name: 'Force (1 Day)',
                    data: forceIndexOneDay,
                    yAxis: 2
                },
                {
                    name: 'Force (13 Day)',
                    data: forceIndexThirteenDay,
                    yAxis: 3
                }
            ]
        });
    });
});