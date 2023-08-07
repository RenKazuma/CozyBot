require("dotenv").config();
const token = process.env.Token;
const prefix = "L!";
require("./deploy-commands");
require("./methods/Poll");
const http = require("http");

const {
  Client,
  GatewayIntentBits,
  Events,
  SlashCommandBuilder,
} = require("discord.js");

const client = new Client({
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMembers,
    GatewayIntentBits.GuildMessages,
    GatewayIntentBits.MessageContent,
  ],
});

client.on("ready", () => {
  console.log("⚪ Ich bin online!");
  // Set the bot's status
  client.user.setPresence({
    activity: { name: "I am now idle", type: "Playing" },
    status: "idle",
  });

  console.log("💖 Status umgestellt");

  //client.channels.fetch('438364905830219817').then(channel => {
  //channel.send("My Message");
  //});

  //client.users.fetch('261136853845934093').then(user => {
  //user.send("This is a test message. PS: I love you");
  //})
});

client.on("messageCreate", async function (message) {
  if (message.author.bot || message.channel.type != 0) {
    return;
  }

  return;
});

// Handle button interactions
client.on('interactionCreate', async (interaction) => {
  if (!interaction.isButton()) return;

  const message = interaction.message;

  var newEmbed;
  if (interaction.customId === "confirm") {
    newEmbed = await Poll.editEmbed(0, interaction);
  } else if (interaction.customId === "cancel"){
     newEmbed = await Poll.editEmbed(1, interaction);
  } else if (interaction.customId === "pending"){
     newEmbed = await Poll.editEmbed(2, interaction);
  }

  if (typeof newEmbed === 'string') {
    await interaction.reply({ content: newEmbed, ephemeral: true });
    return;
  }

      await message.edit({ embeds: [newEmbed] });

      await interaction.reply({ content: `You voted vor ${interaction.customId}`, ephemeral: true }); // Use 'ephemeral: false' for a public reply
    
});

const deploy = require("./deploy-commands");
const { EmbedBuilder } = require("@discordjs/builders");
const Poll = require("./methods/Poll");
deploy.Intizialecommands(client, token);

//If slash command execute slash command
client.on(Events.InteractionCreate, async (interaction) => {
  console.log(
    `${interaction.commandName} was called by ${interaction.user.name} in ${interaction.guild.name}, ${interaction.channel.name}`
  );

  if (!interaction.isChatInputCommand()) return;

  const command = interaction.client.commands.get(interaction.commandName);

  if (!command) {
    console.error(`No command matching ${interaction.commandName} was found.`);
    return;
  }

  if (interaction.deferred || interaction.replied) {
    return;
  }

  try {
    await command.execute(interaction);
    return;
  } catch (error) {
    console.error(error);

    await interaction.reply({
      content: "There was an error while executing this command!",
      ephemeral: true,
    });
    return;
  }
});

client.login(token);

module.exports = {
  client: client,
};
