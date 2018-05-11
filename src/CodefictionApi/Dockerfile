FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM buildpack-deps:jessie-scm AS publisher

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

ENV DOTNET_SDK_VERSION 2.1.200
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz
ENV DOTNET_SDK_DOWNLOAD_SHA c1b07ce8849619ca505aafd2983bcdd7141536ccae243d4249b0c9665daf107e03a696ad5f1d95560142cd841a0888bbf5f1a8ff77d3bdc3696b5873481f0998

RUN curl -SL $DOTNET_SDK_DOWNLOAD_URL --output dotnet.tar.gz \
    && echo "$DOTNET_SDK_DOWNLOAD_SHA dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    NUGET_XMLDOC_MODE=skip

RUN mkdir warmup \
    && cd warmup \
    && dotnet new \
    && cd .. \
    && rm -rf warmup \
    && rm -rf /tmp/NuGetScratch

ENV RestoreUseSkipNonexistentTargets false

ENV NODE_VERSION="8.11.0"

RUN apt-get install wget -y

RUN set -ex \
    && for key in \
      94AE36675C464D64BAFA68DD7434390BDBE9B9C5 \
      0034A06D9D9B0064CE8ADF6BF1747F4AD2306D93 \
      FD3A5288F042B6850C66B31F09FE44734EB7990E \
      71DCFD284A79C3B38668286BC97EC7A07EDE3FC1 \
      DD8F2338BAE7501E3DD5AC78C273792F7D83545D \
      B9AE9905FFD7803F25714661B63B535A4C206CA9 \
      C4F0DFFF4E8C1A8236409D08E73BC641CC11F4C8 \
      77984A986EBC2AA786BC0F66B01FBB92821C587A \
      56730D5401028683275BD23C23EFEFE93C4CFFFE \
    ; do \
      gpg --keyserver ha.pool.sks-keyservers.net --recv-keys "$key"; \
    done

RUN set -ex \
    && wget "https://nodejs.org/download/release/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz" -O node-v$NODE_VERSION-linux-x64.tar.gz \
    && wget "https://nodejs.org/download/release/v$NODE_VERSION/SHASUMS256.txt.asc" -O SHASUMS256.txt.asc \
    && gpg --batch --decrypt --output SHASUMS256.txt SHASUMS256.txt.asc \
    && grep " node-v$NODE_VERSION-linux-x64.tar.gz\$" SHASUMS256.txt | sha256sum -c - \
        && tar -xzf "node-v$NODE_VERSION-linux-x64.tar.gz" -C /usr/local --strip-components=1 \
        && rm "node-v$NODE_VERSION-linux-x64.tar.gz" SHASUMS256.txt.asc SHASUMS256.txt \
        && ln -s /usr/local/bin/node /usr/local/bin/nodejs \
        && rm -fr /var/lib/apt/lists/* /tmp/* /var/tmp/*

RUN npm config set user 0
RUN npm config set unsafe-perm true

RUN npm install -g @angular/cli@1.7.4
RUN node -v && npm -v && ng -v

RUN apt-get update && apt-get install bzip2 -y

ENV ASPNETCORE_URLS http://+:80

WORKDIR /app
COPY . .
RUN dotnet restore ./CodefictionApi
RUN dotnet publish ./CodefictionApi -c Release

FROM base AS final
ARG source
EXPOSE 5102
ENV ASPNETCORE_URLS http://*:80
RUN apt-get -qq update && apt-get -qqy --no-install-recommends install wget gnupg \
    git \
    unzip
RUN curl -sL https://deb.nodesource.com/setup_8.x |  bash -
RUN apt-get install -y nodejs
RUN npm config set user 0
RUN npm config set unsafe-perm true
RUN npm install -g npm

RUN npm install -g @angular/cli@1.7.4 --unsafe
RUN npm install -g @angular/compiler-cli@5.2.1
RUN node -v && npm -v && ng -v

WORKDIR /app/CodefictionApi/bin/Release/netcoreapp2.0/publish
COPY --from=publisher /app/CodefictionApi/bin/Release/netcoreapp2.0/publish .
ENTRYPOINT ["dotnet", "CodefictionApi.dll"]