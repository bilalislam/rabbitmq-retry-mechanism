# -*- mode: ruby -*-
# vi: set ft=ruby :

#!! REQUIRES !! vagrant-hostmanager

# Vagrantfile API/syntax version. Don't touch unless you know what you're doing!
VAGRANTFILE_API_VERSION = '2'

BOXES = [
  { name: :mq1, ip: '192.168.77.10',guest:'15672',host:'15672', },
  { name: :mq2, ip: '192.168.77.11',guest:'15672',host:'15672', },
  { name: :mq3, ip: '192.168.77.12',guest:'15672',host:'15672', }
]

Vagrant.configure(VAGRANTFILE_API_VERSION) do |config|
  config.ssh.insert_key = false
  config.vm.box = 'minimal/centos7'
  config.ssh.private_key_path = "~/.vagrant.d/insecure_private_key"
  config.ssh.forward_agent = true

  config.vm.provider :virtualbox do |vb|
    vb.customize ['modifyvm', :id, '--cpus', '1']
    vb.customize ['modifyvm', :id, '--memory', '1024']
    vb.customize ['modifyvm', :id, '--natdnshostresolver1', 'on']
    vb.customize ["modifyvm", :id, "--usb", "on"]
    vb.customize ["modifyvm", :id, "--usbehci", "off"]
  end

  BOXES.each do |opts|
    config.vm.define opts[:name] do |config|
      config.vm.hostname = opts[:name].to_s
      config.vm.network "public_network" , ip: opts[:ip]
      #config.vm.network "forwarded_port" , guest: opts[:guest], host: opts[:host]
      config.vm.provision "ansible" do |ansible|
         ansible.playbook = "rabbitmq-cluster.yml"
      end
    end
  end
end