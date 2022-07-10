function  gerarPayload()  {

    var data = SpreadsheetApp.getActiveSheet().getDataRange().getValues();
    data.shift();
    var list = []
    for (row in data) {

        var row = data[row]

        var payload = {
            nome: row[0],
            email: row[1],
            dataNascimento: row[2]
        }; 

        list.push(payload);
     }

       var options = {
              method: "post",
              contentType: "application/json", // contentType property was mistyped as ContentType - case matters
              payload: JSON.stringify(list)
        }; 

        Logger.log(options);

        UrlFetchApp.fetch('http://194.163.144.250:5000/planilha', options);

}

