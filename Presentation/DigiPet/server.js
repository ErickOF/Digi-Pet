const express = require('express');
const app = express();

const path = require('path');

app.use(express.static(__dirname + '/dist/DigiPet'));
app.listen(process.env.PORT || 8080);
app.get('*', function(req, res) {
	resp.sendFile(path.join(__dirname + '/dist/DigiPet/index.html'));
});

console.log('Listenting!');
