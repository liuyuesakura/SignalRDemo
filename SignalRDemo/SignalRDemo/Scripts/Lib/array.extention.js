Array.prototype.indexOf = function (val) {
	for (var i = 0; i < this.length; i++) {
		if (this[i] == val) return i;
		for (var j in this[i]) {
			if (this[i][j] == val) return i;
		}
	}
	return -1;
}
Array.prototype.indexOf = function (val, col) {
	for (var i = 0; i < this.length; i++) {
		if (this[i] == val) return i;
		for (var j in this[i]) {
			if (col != undefined && j.toLowerCase() != col.toLowerCase())
				continue;
			if (this[i][j] == val) return i;
		}
	}
	return -1;
}