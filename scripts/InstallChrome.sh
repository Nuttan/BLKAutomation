#!/bin/bash
echo "chrome install success"
apt-get update -y
apt-get install -y gnupg2
apt-get upgrade -y
apt purge google-chrome-stable
set -ex
apt-get -f install
apt install fonts-liberation
gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 4EB27DB2A3B88B8B
echo "deb http://dl.google.com/linux/chrome/deb/ stable main" |  tee /etc/apt/sources.list.d/google.list
apt-get update && apt-get install -y google-chrome-stable
