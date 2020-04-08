ifeq ($(OS),Windows_NT)
all : clean build test 
else
all : clean secp256k1 build test
endif

secp256k1:
	git clone https://github.com/MeadowSuite/secp256k1
	cd secp256k1 && ./autogen.sh && ./configure && chmod +x ./build-linux.sh && ./build-linux.sh
	cp secp256k1/build/libsecp256k1.so core/libsecp256k1.so

build:
	dotnet build core/BGC --configuration Release -o build
	cp core/libsecp256k1.so build
clean:
	rm -rf build
test:
	dotnet test core/BGC.Tests
