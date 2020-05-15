FROM mcr.microsoft.com/dotnet/core/sdk:3.1
RUN apt update 
RUN apt install make autoconf cmake libtool -y
COPY . /BGC

WORKDIR /BGC

RUN make

ENTRYPOINT ["/BGC/build/BGC"]
CMD ["help"]
