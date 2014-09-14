
var symbol = $('#symbol').attr('value');

$(function () {

    $("#symbol-search").autocomplete({
            source: "/Ajax/GetMatchingCompanies"
    });                 
  

    $.getJSON('/api/ChartResource/Get/?symbol=' + symbol, function(data) {

        // split the data set into ohlc and volume
        var ohlc = [],
            volume = [],
            forceIndexThirteenDay = [],
            upperBollingerBandTwoDeviation = [],
            upperBollingerBandOneDeviation = [],
            lowerBollingerBandTwoDeviation = [],
            lowerBollingerBandOneDeviation = [],
            macd = [],
            macdSignalLine = [],
            macdHistogram = [],
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

            forceIndexThirteenDay.push([
                data.Data[i][0],
                data.Data[i][6]
            ]);
             
            upperBollingerBandTwoDeviation.push([
                data.Data[i][0],
                data.Data[i][7]
            ]);
           
            lowerBollingerBandTwoDeviation.push([
                data.Data[i][0],
                data.Data[i][8]
            ]);

            upperBollingerBandOneDeviation.push([
                data.Data[i][0],
                data.Data[i][9]
            ]);

            lowerBollingerBandOneDeviation.push([
                data.Data[i][0],
                data.Data[i][10]
            ]);
            
            macd.push([
                data.Data[i][0],
                data.Data[i][11]
            ]);
            
            macdSignalLine.push([
                data.Data[i][0],
                data.Data[i][12]
            ]);
            
            macdHistogram.push([
                data.Data[i][0],
                data.Data[i][13]
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
                { name: 'MACD', data: macd, yAxis: 2 },
                { name: 'MACD Signal Line', data: macdSignalLine, yAxis: 2 },
                { name: 'MACD Histogram', type: 'column', data: macdHistogram, yAxis: 2 },
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
                    top: '41%',
                    height: '7%',
                    offset: 0,
                    lineWidth: 2
                },
                                {
                                    labels: {
                                        align: 'right',
                                        x: -3
                                    },
                                    title: {
                                        text: 'MACD'
                                    },
                                    top: '50%',
                                    height: '12%',
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
                    top: '64%',
                    height: '20%',
                    offset: 0,
                    lineWidth: 2
                }
            ]
        });
    });
});