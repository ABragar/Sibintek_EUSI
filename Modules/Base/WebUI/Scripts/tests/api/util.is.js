describe('api/util.is.js', function() {

    describe('#isEmpty', function() {

        takeAsEmpty('`null`', null);
        takeAsEmpty('`undefined`', undefined);
        takeAsEmpty('an object without properties', {});
        takeAsEmpty('a function', function() { });

        it('Should take object with properties as it\'s not empty', function() {
            expect(pbaAPI.isEmpty({ a: 5 })).to.equal(false);
        });

        function takeAsEmpty(name, val, asWhat) {
            it('Should take ' + name + ' as empty', function() {
                expect(pbaAPI.isEmpty(val)).to.equal(true);
            });
        }
    });

    describe('#isFunction', function() {

        it('Should determine a function', function() {
            expect(pbaAPI.isFunction(function() { })).to.equal(true);
        });

        it('Should determine a not-function', function() {
            Test.each.notFunction(function(notFunction) {
                expect(pbaAPI.isFunction(notFunction)).to.equal(false);
            });
        });

    });

    describe('#isArray', function() {

        it('Should determine an array', function() {
            expect(pbaAPI.isArray([])).to.equal(true);
        });

        it('Should determine a not-array', function() {
            var arrayLike = {
                0: 'abc',
                length: 1
            };

            expect(pbaAPI.isArray(arrayLike)).to.equal(false);
        });

    });

    describe('#isArrayLike', function() {

        it('Should determine an "array-like"', function() {
            var arrayLike = {
                0: 'abc',
                length: 1
            };

            expect(pbaAPI.isArrayLike(arrayLike)).to.equal(true);
        });

        it('Should determine a not-"array-like"', function() {
            var notArrayLike = {
                0: 'abc',
                1: 123
            };

            expect(pbaAPI.isArrayLike(notArrayLike)).to.equal(false);
        });

    });

});
