// Load the fonts
Highcharts.createElement('link', {
    href: 'http://fonts.googleapis.com/css?family=Unica+One',
    rel: 'stylesheet',
    type: 'text/css'
}, null, document.getElementsByTagName('head')[0]);

Highcharts.theme = {
    colors: ["#2b908f", "#90ee7e", "#f45b5b", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
       "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
    chart: {
        backgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 },
            stops: [
               [0, '#2a2a2b'],
               [1, '#3e3e40']
            ]
        },
        style: {
            fontFamily: "'Unica One', sans-serif"
        },
        plotBorderColor: '#606063'
    },
    title: {
        style: {
            color: '#E0E0E3',
            textTransform: 'uppercase',
            fontSize: '20px'
        }
    },
    subtitle: {
        style: {
            color: '#E0E0E3',
            textTransform: 'uppercase'
        }
    },
    xAxis: {
        gridLineColor: '#707073',
        labels: {
            style: {
                color: '#E0E0E3'
            }
        },
        lineColor: '#707073',
        minorGridLineColor: '#505053',
        tickColor: '#707073',
        title: {
            style: {
                color: '#A0A0A3'

            }
        }
    },
    yAxis: {
        gridLineColor: '#707073',
        labels: {
            style: {
                color: '#E0E0E3'
            }
        },
        lineColor: '#707073',
        minorGridLineColor: '#505053',
        tickColor: '#707073',
        tickWidth: 1,
        title: {
            style: {
                color: '#A0A0A3'
            }
        }
    },
    tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.85)',
        style: {
            color: '#F0F0F0'
        }
    },
    plotOptions: {
        series: {
            dataLabels: {
                color: '#B0B0B3'
            },
            marker: {
                lineColor: '#333'
            }
        },
        boxplot: {
            fillColor: '#505053'
        },
        candlestick: {
            lineColor: 'white'
        },
        errorbar: {
            color: 'white'
        }
    },
    legend: {
        itemStyle: {
            color: '#E0E0E3'
        },
        itemHoverStyle: {
            color: '#FFF'
        },
        itemHiddenStyle: {
            color: '#606063'
        }
    },
    credits: {
        style: {
            color: '#666'
        }
    },
    labels: {
        style: {
            color: '#707073'
        }
    },

    drilldown: {
        activeAxisLabelStyle: {
            color: '#F0F0F3'
        },
        activeDataLabelStyle: {
            color: '#F0F0F3'
        }
    },

    navigation: {
        buttonOptions: {
            symbolStroke: '#DDDDDD',
            theme: {
                fill: '#505053'
            }
        }
    },

    // scroll charts
    rangeSelector: {
        buttonTheme: {
            fill: '#505053',
            stroke: '#000000',
            style: {
                color: '#CCC'
            },
            states: {
                hover: {
                    fill: '#707073',
                    stroke: '#000000',
                    style: {
                        color: 'white'
                    }
                },
                select: {
                    fill: '#000003',
                    stroke: '#000000',
                    style: {
                        color: 'white'
                    }
                }
            }
        },
        inputBoxBorderColor: '#505053',
        inputStyle: {
            backgroundColor: '#333',
            color: 'silver'
        },
        labelStyle: {
            color: 'silver'
        }
    },

    navigator: {
        handles: {
            backgroundColor: '#666',
            borderColor: '#AAA'
        },
        outlineColor: '#CCC',
        maskFill: 'rgba(255,255,255,0.1)',
        series: {
            color: '#7798BF',
            lineColor: '#A6C7ED'
        },
        xAxis: {
            gridLineColor: '#505053'
        }
    },

    scrollbar: {
        barBackgroundColor: '#808083',
        barBorderColor: '#808083',
        buttonArrowColor: '#CCC',
        buttonBackgroundColor: '#606063',
        buttonBorderColor: '#606063',
        rifleColor: '#FFF',
        trackBackgroundColor: '#404043',
        trackBorderColor: '#404043'
    },

    // special colors for some of the
    legendBackgroundColor: 'rgba(0, 0, 0, 0.5)',
    background2: '#505053',
    dataLabelsColor: '#B0B0B3',
    textColor: '#C0C0C0',
    contrastTextColor: '#F0F0F3',
    maskColor: 'rgba(255,255,255,0.3)'
};

// Apply the theme
Highcharts.setOptions(Highcharts.theme);


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
var symbol = $('#symbol').attr('value');

$(function () {
    $.getJSON('http://localhost:53110/api/ChartResource/Get/?symbol=' + symbol, function(data) {

        // split the data set into ohlc and volume
        var ohlc = [],
            volume = [],
            forceIndexOneDay = [],
            forceIndexThirteenDay = [],
            upperBollingerBandTwoDeviation = [],
            upperBollingerBandOneDeviation = [],
            lowerBollingerBandTwoDeviation = [],
            lowerBollingerBandOneDeviation = [],
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
             
            upperBollingerBandTwoDeviation.push([
                data.Data[i][0],
                data.Data[i][8]
            ]);
           
            lowerBollingerBandTwoDeviation.push([
                data.Data[i][0],
                data.Data[i][9]
            ]);

            upperBollingerBandOneDeviation.push([
                data.Data[i][0],
                data.Data[i][10]
            ]);

            lowerBollingerBandOneDeviation.push([
                data.Data[i][0],
                data.Data[i][11]
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
            rangeSelector: { inputEnabled: $('#container').width() > 480, selected: 1 },

            title: { text: symbol },

            series: [
                { type: 'candlestick', name: symbol, data: ohlc },
                { name: 'Upper Bollinger Band 2 Deviation', data: upperBollingerBandTwoDeviation, color: '#FF0000' },
                { name: 'Lower Bollinger Band 2 Deviation', data: lowerBollingerBandTwoDeviation, color: '#FF0000' },
                { name: 'Upper Bollinger Band 1 Deviation', data: upperBollingerBandOneDeviation, color: '#FF0000' },
                { name: 'Lower Bollinger Band 1 Deviation', data: lowerBollingerBandOneDeviation, color: '#FF0000' },
                { type: 'column', name: 'Volume', data: volume, yAxis: 1 },
                { name: 'Force (1 Day)', data: forceIndexOneDay, yAxis: 2 },
                { name: 'Force (13 Day)', data: forceIndexThirteenDay, yAxis: 3 }
            ],

            plotOptions: {
                candlestick: {
                    color: 'red',
                    upColor: 'green'
                }
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
            ]
        });
    });
});