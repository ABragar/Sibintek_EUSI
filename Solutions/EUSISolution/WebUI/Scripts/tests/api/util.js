describe('api/util.js', function() {

    describe('#async', function() {

        it('Should throw if first argument is not a function', function() {
            Test.each.notFunction(function(notFunction) {
                expect(function() {
                    pbaAPI.async(notFunction);
                }).to.throw(Error);
            });
        });

        it('Should execute function in asynchronously', function(done) {
            var some = 1;

            pbaAPI.async(function() {
                some = 2;
            });

            expect(some).to.equal(1);

            setTimeout(function() {
                expect(some).to.equal(2);

                done();
            }, 0);
        });

    });

    describe('#uid', function() {

        it('Should return pseudo-unique strings', function() {
            var count = 100;
            var dict = {};

            for (var i = 0; i < count; i++) {
                dict[pbaAPI.uid()] = true;
            }

            expect(Object.keys(dict)).to.have.length(count);
        });

        it('Should prepend given prefix', function() {
            var prefix = 'some random prefix';
            var result = pbaAPI.uid(prefix);

            expect(result.indexOf(prefix + '_')).to.equal(0);
        });

    });

    describe('#guid', function() {

        var regGuid = /[a-f\d]{8}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{12}/i;

        it('Should return pseudo-unique strings', function() {
            var count = 100;
            var dict = {};

            for (var i = 0; i < count; i++) {
                dict[pbaAPI.guid()] = true;
            }

            expect(Object.keys(dict)).to.have.length(count);
        });

        it('Should prepend given prefix', function() {
            var prefix = 'some random prefix';
            var result = pbaAPI.guid(prefix);

            expect(result.indexOf(prefix + '_')).to.equal(0);
        });

        it('Should be guid-like', function() {
            var PREFIX = 'prefix';

            var result = pbaAPI.guid();
            var prefixed = pbaAPI.guid(PREFIX);
            var unprefixed = prefixed.slice(PREFIX.length + 1);

            expect(regGuid.test(result)).to.equal(true);
            expect(regGuid.test(unprefixed)).to.equal(true);
        });

    });

    describe('#truncate', function() {

        var STRING = 'some string to truncate';

        it('Should return an empty string, if first argument is falsy', function() {
            Test.each.falsy(function(falsy) {
                var result = pbaAPI.truncate(falsy);

                expect(result).to.equal('');
            });
        });

        it('Should return the same string, if second argument is falsy', function() {
            Test.each.falsy(function(falsy) {
                var result = pbaAPI.truncate(STRING, falsy);

                expect(result).to.equal(STRING);
            });
        });

        it('Should return the same string, if second argument is bigger than string length', function() {
            var result = pbaAPI.truncate(STRING, STRING.length + 1);

            expect(result).to.equal(STRING);
        });

        it('Should truncate string to given length', function() {
            var givenStr = 'some string to truncate';
            var expectStr = 'some string to...';
            var truncateTo = 18;

            var result = pbaAPI.truncate(givenStr, truncateTo);

            expect(result).to.equal(expectStr);
        });

    });

    // TODO: #splitTextIntoLines

    describe('#distinct', function() {

        var REF =   [1, 2, 3, 2, 1];
        var ARRAY = [1, 2, 3, 2, 1];
        var ARRAY_LIKE = {
            0: 1,
            1: 2,
            2: 3,
            3: 2,
            4: 1,
            length: 5
        };

        it('Should return given argument, if it\'s not an array and not an "array-like"', function() {
            var given = { a: 5 };
            var result = pbaAPI.distinct(given);

            expect(result).to.equal(given);
        });

        it('Should remove dublicates from array', function() {
            var result = pbaAPI.distinct(ARRAY);

            expect(result).to.have.length(3);
                
            expect(result[0]).to.equal(1);
            expect(result[1]).to.equal(2);
            expect(result[2]).to.equal(3);
        });

        it('Should remove dublicates from "array-like"', function() {
            var result = pbaAPI.distinct(ARRAY_LIKE);

            expect(result).to.have.length(3);
            expect(result[0]).to.equal(1);
            expect(result[1]).to.equal(2);
            expect(result[2]).to.equal(3);
        });

        it('Should return an array if argument is "array-like"', function() {
            var result = pbaAPI.distinct(ARRAY_LIKE);

            expect(Array.isArray(result)).to.equal(true);
        });

        it('Should not mutate given object', function() {
            var result = pbaAPI.distinct(ARRAY);

            expect(ARRAY).to.not.equal(result);
            expect(ARRAY).to.have.length(REF.length);
                
            for (var i = 0; i < ARRAY.length; i++) {
                expect(ARRAY[i]).to.equal(REF[i]);
            }
        });

    });

    describe('#distinctBy', function() {

        var REF = [
            { a: 1, b: 1 },
            { a: 2, b: 2 },
            { a: 3, b: 3 },
            { a: 2, b: 4 },
            { a: 1, b: 5 },
        ];

        var ARRAY = [
            { a: 1, b: 1 },
            { a: 2, b: 2 },
            { a: 3, b: 3 },
            { a: 2, b: 4 },
            { a: 1, b: 5 },
        ];

        var ARRAY_LIKE = {
            0: { a: 1, b: 1 },
            1: { a: 2, b: 2 },
            2: { a: 3, b: 3 },
            3: { a: 2, b: 4 },
            4: { a: 1, b: 5 },
            length: 5
        };

        it('Should return given argument, if it\'s not an array and not an "array-like"', function() {
            var given = { a: 5 };
            var result = pbaAPI.distinctBy(given);

            expect(result).to.equal(given);
        });

        it('Should remove dublicates from array', function() {
            var result1 = pbaAPI.distinctBy(ARRAY, 'a');
            var result2 = pbaAPI.distinctBy(ARRAY, 'b');

            expect(result1).to.have.length(3);
            expect(result1[0].a).to.equal(1);
            expect(result1[1].a).to.equal(2);
            expect(result1[2].a).to.equal(3);

            expect(result2).to.have.length(ARRAY.length);

            for (var i = 0; i < result2.length; i++) {
                expect(result2[i].b).to.equal(ARRAY[i].b);
            }
        });

        it('Should remove dublicates from "array-like"', function() {
            var result1 = pbaAPI.distinctBy(ARRAY_LIKE, 'a');
            var result2 = pbaAPI.distinctBy(ARRAY_LIKE, 'b');

            expect(result1).to.have.length(3);
            expect(result1[0].a).to.equal(1);
            expect(result1[1].a).to.equal(2);
            expect(result1[2].a).to.equal(3);

            expect(result2).to.have.length(ARRAY_LIKE.length);

            for (var i = 0; i < result2.length; i++) {
                expect(result2[i].b).to.equal(ARRAY_LIKE[i].b);
            }
        });

        it('Should return an array if argument is "array-like"', function() {
            var result = pbaAPI.distinctBy(ARRAY_LIKE, 'a');

            expect(Array.isArray(result)).to.equal(true);
        });

        it('Should not mutate a given object', function() {
            var result = pbaAPI.distinctBy(ARRAY, 'a');

            expect(ARRAY).to.not.equal(result);
            expect(ARRAY).to.have.length(REF.length);

            for (var i = 0; i < ARRAY.length; i++) {
                var obj1 = ARRAY[i];
                var obj2 = REF[i];

                expect(Object.keys(obj1)).to.have.length(Object.keys(obj2).length);
                expect(obj1.a).to.equal(obj2.a);
                expect(obj1.b).to.equal(obj2.b);
            }
        });

        it('Should work like #distinct if second argument is absent', function() {
            var result = pbaAPI.distinctBy([2, 1, 1, 3]);

            expect(Array.isArray(result)).to.equal(true);
            expect(result).to.have.length(3);
            expect(result[0]).to.equal(2);
            expect(result[1]).to.equal(1);
            expect(result[2]).to.equal(3);
        });

    });

    // TODO: #each
    // TODO: #filter
    // TODO: #map
    // TODO: #reduce

    describe('#extract', function() {

        var ARRAY = [
            { a: 1, b: 2 },
            { a: 2 },
            { a: 3, b: 'abc' },
            { a: 4 }
        ];

        var ARRAY_LIKE = {
            0: { a: 1, b: 2 },
            1: { a: 2 },
            2: { a: 3, b: 'abc' },
            3: { a: 4 },
            length: 4
        };

        it('Should return an empty array, if given object is not an array or "array-like"', function() {
            var result = pbaAPI.extract({});

            expect(Array.isArray(result)).to.equal(true);
            expect(result).to.have.length(0);
        });

        it('Should work with array', function() {
            var result = pbaAPI.extract(ARRAY, 'a');

            expect(result).to.have.length(ARRAY.length);
                
            for (var i = 0; i < result.length; i++) {
                expect(result[i]).to.equal(ARRAY[i].a);
            }
        });

        it('Should work with "array-like"', function() {
            var result = pbaAPI.extract(ARRAY_LIKE, 'a');

            expect(result).to.have.length(ARRAY_LIKE.length);

            for (var i = 0; i < result.length; i++) {
                expect(result[i]).to.equal(ARRAY_LIKE[i].a);
            }
        });

        it('Should return `undefined` on absent properties by default', function() {
            var result = pbaAPI.extract(ARRAY, 'b');

            expect(result).to.have.length(ARRAY.length);

            for (var i = 0; i < result.length; i++) {
                expect(result[i]).to.equal(ARRAY[i].hasOwnProperty('b') ? ARRAY[i].b : undefined);
            }
        });

        it('Should exclude properties which is absent in object if third argument (skipWrong) is `true`', function() {
            var result = pbaAPI.extract(ARRAY, 'b', true);

            expect(result).to.have.length(2);
            expect(result[0]).to.equal(2);
            expect(result[1]).to.equal('abc');
        });

    });

    describe('json', function() {

        describe('#parse', function() {

            it('Should not throw if json is invalid', function() {
                var invalidJsons = ['', undefined, null, '{ some: 7 ]}'];
                var count = 0;

                invalidJsons.forEach(function(invalidJson) {
                    try {
                        pbaAPI.json.parse(invalidJson);
                        count++;
                    } catch (e) { /*EMPTY*/ }
                });

                expect(count).to.equal(invalidJsons.length);
            });

            it('Should return `null` if json is invalid', function() {
                var invalidJsons = ['', undefined, null, '{ some: 7 ]}'];
                var results = [];

                invalidJsons.forEach(function(invalidJson) {
                    try {
                        results.push(pbaAPI.json.parse(invalidJson));
                    } catch (e) { /*EMPTY*/ }
                });

                expect(results).to.have.length(invalidJsons.length);

                results.forEach(function(result) {
                    expect(result).to.equal(null);
                });
            });

            it('Should parse valid json', function() {
                var obj = { a: 5 };
                var json = JSON.stringify(obj);
                var result = pbaAPI.json.parse(json);

                expect(result).to.be.an('object');
                expect(result.a).to.equal(5);
            });

        });

        describe('#stringify', function() {

            it('Should not throw if object could not be stringified', function() {
                var badObjects = ['', null, { a: 5, b: badObjects/*circular reference*/ }];
                var count = 0;

                badObjects.forEach(function(obj) {
                    try {
                        pbaAPI.json.stringify(obj);
                        count++;
                    } catch (e) { /*EMPTY*/ }
                });

                expect(count).to.equal(badObjects.length);
            });

            it('Should stringify to a valid json', function() {
                var obj = [ 1, 'ab' ];
                var json = JSON.stringify(obj);

                var result = pbaAPI.json.stringify(obj);

                expect(result).to.be.a('string');
                expect(result).to.equal(json);
            });

        });

    });

    // TODO: #emitterMixin

    describe('#htmlEncode', function() {

        it('Should return the same given argument, if it\'s falsy', function() {
            Test.each.falsy(function(falsy) {
                if (isNaN(falsy)) {
                    expect(isNaN(pbaAPI.htmlEncode(falsy))).to.equal(true);
                } else {
                    expect(pbaAPI.htmlEncode(falsy)).to.equal(falsy);
                }
            });
        });

        checkEncode('&', '&amp;', 'l & m', 'l &amp; m');
        checkEncode('<', '&lt;', '<div', '&lt;div');
        checkEncode('>', '&gt;', '/div>', '/div&gt;');
        checkEncode('"', '&quot;', 'some="123"', 'some=&quot;123&quot;');
        checkEncode('\'', '&apos;', 'attr=\'123\'', 'attr=&apos;123&apos;');

        function checkEncode(cRaw, cEnc, sRaw, sEnc) {
            it('Should encode ' + cRaw + ' to ' + cEnc, function() {
                expect(pbaAPI.htmlEncode(sRaw)).to.equal(sEnc);
            });
        }
    });
});
