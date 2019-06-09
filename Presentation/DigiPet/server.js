const expreses = require('express');
const path = require('path');

const app = express();
app.use(express.static(__dirname + 'dist'));
app.listen(process.env.PORT || 10000);
app.get('/*', function(request, response) {
	response.sendFile(path.join(__dirname + '/dist/index.html'));
});

console.log('Listenting!');
