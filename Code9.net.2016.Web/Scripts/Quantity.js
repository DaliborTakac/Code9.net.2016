var Quantity = function (initialValue, minValue, maxValue, updateCallback) {
    var val = initialValue;

    this.decrease = function () {
        if (val > minValue) {
            --val;
            if (updateCallback) {
                updateCallback(val);
            }
        }
    }

    this.increase = function () {
        if (val < maxValue) {
            ++val;
            if (updateCallback) {
                updateCallback(val);
            }
        }
    }

    this.reset = function () {
        val = initialValue;
        if (updateCallback) {
            updateCallback(val);
        }
    }

    this.value = function () {
        return val;
    }
};
