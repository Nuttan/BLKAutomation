#!/bin/bash
echo "chrome install success"
set -ex
wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
sudo apt-get update
sudo dpkg -i google-chrome-stable_current_amd64.deb
sudo apt-get install -f
